namespace NutriScan.Api.DTOs;

public record UserLogDTO(
    string Barcode,
    decimal Quantity,
    string Meal
);
