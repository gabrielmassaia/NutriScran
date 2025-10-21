using Microsoft.AspNetCore.Mvc;
using NutriScan.Api.DTOs;
using NutriScan.Api.Services;

namespace NutriScan.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly SupabaseService _supabaseService;
    private readonly ProductService _productService;

    public UsersController(SupabaseService supabaseService, ProductService productService)
    {
        _supabaseService = supabaseService;
        _productService = productService;
    }

    [HttpPost("{id:guid}/logs")]
    public async Task<IActionResult> InsertLog(Guid id, [FromBody] UserLogDTO dto)
    {
        await _supabaseService.InsertLogAsync(id, dto);
        return Accepted();
    }

    [HttpGet("{id:guid}/logs")]
    public async Task<IActionResult> GetLogs(Guid id)
    {
        var logs = await _supabaseService.GetLogsAsync(id);
        var enriched = new List<object>();
        foreach (var log in logs)
        {
            var product = await _productService.GetProductAsync(log.Barcode);
            enriched.Add(new
            {
                log.Id,
                log.Barcode,
                log.Meal,
                log.Quantity,
                log.ScannedAt,
                Product = product is null
                    ? null
                    : new
                    {
                        product.Name,
                        product.Brand,
                        product.HealthScore,
                        product.Nutrients
                    }
            });
        }

        return Ok(enriched);
    }
}
