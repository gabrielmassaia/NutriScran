using System.Globalization;
using System.Net.Http.Json;
using Microsoft.Extensions.Caching.Memory;
using NutriScan.Api.Models;

namespace NutriScan.Api.Services;

public class OpenFoodFactsService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly ILogger<OpenFoodFactsService> _logger;

    public OpenFoodFactsService(HttpClient httpClient, IMemoryCache cache, ILogger<OpenFoodFactsService> logger)
    {
        _httpClient = httpClient;
        _cache = cache;
        _logger = logger;
    }

    public async Task<Product?> FetchProductAsync(string barcode)
    {
        if (string.IsNullOrWhiteSpace(barcode))
        {
            return null;
        }

        if (_cache.TryGetValue<Product>(barcode, out var cached))
        {
            return cached;
        }

        try
        {
            var response = await _httpClient.GetFromJsonAsync<OpenFoodFactsResponse>($"{barcode}.json");
            if (response?.Product == null)
            {
                return null;
            }

            var product = MapToProduct(response.Product, barcode);
            _cache.Set(barcode, product, TimeSpan.FromMinutes(15));
            return product;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falha ao consultar OpenFoodFacts para {Barcode}", barcode);
            return null;
        }
    }

    private static Product MapToProduct(OpenFoodFactsProduct source, string barcode)
    {
        var nutrients = new Nutrients
        {
            Calories = ParseDecimal(source.Nutriments?.EnergyKcal100g),
            Protein = ParseDecimal(source.Nutriments?.Proteins100g),
            Fat = ParseDecimal(source.Nutriments?.Fat100g),
            Sugar = ParseDecimal(source.Nutriments?.Sugars100g),
            Sodium = ParseDecimal(source.Nutriments?.Sodium100g) is { } sodium ? sodium * 1000 : null,
            Carbohydrates = ParseDecimal(source.Nutriments?.Carbohydrates100g),
            Fiber = ParseDecimal(source.Nutriments?.Fiber100g),
            SaturatedFat = ParseDecimal(source.Nutriments?.SaturatedFat100g)
        };

        return new Product
        {
            Id = barcode,
            Barcode = barcode,
            Name = source.ProductName ?? source.GenericName ?? "Produto OpenFoodFacts",
            Brand = source.Brands?.Split(',').FirstOrDefault()?.Trim(),
            ImageUrl = source.ImageUrl,
            Nutrients = nutrients,
            Tags = source.CategoriesTags,
            Benefits = source.LabelsTags?.Where(tag => tag.Contains("bio", StringComparison.OrdinalIgnoreCase) || tag.Contains("natural", StringComparison.OrdinalIgnoreCase)).ToArray(),
            Malefits = source.AdditivesTags,
            HealthScore = HealthScoreCalculator.Calculate(nutrients),
            Source = "OpenFoodFacts",
            LastUpdated = DateTime.UtcNow
        };
    }

    private static decimal? ParseDecimal(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        return decimal.TryParse(value, NumberStyles.Number, CultureInfo.InvariantCulture, out var parsed)
            ? parsed
            : null;
    }

    private record OpenFoodFactsResponse(OpenFoodFactsProduct? Product);

    private record OpenFoodFactsProduct(
        string? ProductName,
        string? GenericName,
        string? Brands,
        string? ImageUrl,
        string[]? CategoriesTags,
        string[]? LabelsTags,
        string[]? AdditivesTags,
        OpenFoodFactsNutriments? Nutriments
    );

    private record OpenFoodFactsNutriments(
        string? EnergyKcal100g,
        string? Proteins100g,
        string? Fat100g,
        string? Sugars100g,
        string? Sodium100g,
        string? Carbohydrates100g,
        string? Fiber100g,
        string? SaturatedFat100g
    );
}

public static class HealthScoreCalculator
{
    public static int Calculate(Nutrients nutrients)
    {
        var score = 100m;

        if (nutrients.Sugar is { } sugar)
        {
            score -= Math.Clamp(sugar * 2, 0, 40);
        }

        if (nutrients.Sodium is { } sodium)
        {
            score -= Math.Clamp(sodium / 10, 0, 25);
        }

        if (nutrients.SaturatedFat is { } saturatedFat)
        {
            score -= Math.Clamp(saturatedFat * 5, 0, 20);
        }

        if (nutrients.Fiber is { } fiber)
        {
            score += Math.Clamp(fiber * 3, 0, 15);
        }

        if (nutrients.Protein is { } protein)
        {
            score += Math.Clamp(protein * 2, 0, 20);
        }

        return (int)Math.Clamp(score, 0, 100);
    }
}
