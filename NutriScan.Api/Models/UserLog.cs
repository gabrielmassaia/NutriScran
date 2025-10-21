namespace NutriScan.Api.Models;

public class UserLog
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Barcode { get; set; } = default!;
    public DateTime ScannedAt { get; set; }
    public decimal Quantity { get; set; }
    public string Meal { get; set; } = "snack";
}
