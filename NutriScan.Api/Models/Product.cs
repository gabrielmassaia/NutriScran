using MongoDB.Bson.Serialization.Attributes;

namespace NutriScan.Api.Models;

public class Product
{
    [BsonId]
    [BsonElement("_id")]
    public string Id { get; set; } = default!;

    public string Barcode { get; set; } = default!;
    public string Name { get; set; } = default!;
    public string? Brand { get; set; }
    public string? ImageUrl { get; set; }
    public Nutrients Nutrients { get; set; } = new();
    public IEnumerable<string>? Tags { get; set; }
    public IEnumerable<string>? Benefits { get; set; }
    public IEnumerable<string>? Malefits { get; set; }
    public int HealthScore { get; set; }
    public string Source { get; set; } = "seed";
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
