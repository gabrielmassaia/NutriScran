using Microsoft.AspNetCore.Mvc;
using NutriScan.Api.DTOs;
using NutriScan.Api.Services;

namespace NutriScan.Api.Controllers;

[ApiController]
[Route("api/stats")]
public class StatsController : ControllerBase
{
    private readonly SupabaseService _supabaseService;
    private readonly ProductService _productService;

    public StatsController(SupabaseService supabaseService, ProductService productService)
    {
        _supabaseService = supabaseService;
        _productService = productService;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserStatsDTO>> Get(Guid id)
    {
        var logs = await _supabaseService.GetLogsAsync(id);
        var today = DateTime.UtcNow.Date;
        var todaySummary = new Dictionary<string, decimal>
        {
            ["calories"] = 0,
            ["protein"] = 0,
            ["fat"] = 0,
            ["sugar"] = 0
        };

        var weekly = new Dictionary<DateTime, decimal>();

        foreach (var log in logs)
        {
            var product = await _productService.GetProductAsync(log.Barcode);
            if (product is null)
            {
                continue;
            }

            var nutrients = product.Nutrients;
            var day = log.ScannedAt.Date;
            var calories = (nutrients.Calories ?? 0) * log.Quantity;
            weekly[day] = weekly.GetValueOrDefault(day) + calories;

            if (day == today)
            {
                todaySummary["calories"] += calories;
                todaySummary["protein"] += (nutrients.Protein ?? 0) * log.Quantity;
                todaySummary["fat"] += (nutrients.Fat ?? 0) * log.Quantity;
                todaySummary["sugar"] += (nutrients.Sugar ?? 0) * log.Quantity;
            }
        }

        var weeklySeries = weekly
            .OrderBy(kvp => kvp.Key)
            .Select(kvp => new DailyCalories(kvp.Key.ToString("ddd"), kvp.Value))
            .ToList();

        return Ok(new UserStatsDTO(todaySummary, weeklySeries));
    }
}
