using NutriScan.Api.Models;

namespace NutriScan.Api.DTOs;

public record ProductDTO(
    string Barcode,
    string Name,
    string? Brand,
    string? ImageUrl,
    Nutrients Nutrients,
    IEnumerable<string>? Benefits,
    IEnumerable<string>? Malefits,
    int HealthScore,
    string Source,
    DateTime LastUpdated
);
