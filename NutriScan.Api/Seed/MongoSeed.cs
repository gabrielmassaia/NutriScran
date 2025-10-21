using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NutriScan.Api.Data;
using NutriScan.Api.Models;

namespace NutriScan.Api.Seed;

public static class MongoSeed
{
    public static async Task RunAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<MongoContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("MongoSeed");
        var collection = context.Products;

        var seeds = GetProducts();

        foreach (var product in seeds)
        {
            await collection.ReplaceOneAsync(p => p.Id == product.Id, product, new ReplaceOptions { IsUpsert = true });
            logger.LogInformation("Produto seed {Barcode} sincronizado", product.Barcode);
        }
    }

    private static IEnumerable<Product> GetProducts()
    {
        return new List<Product>
        {
            new()
            {
                Id = "7891000055123",
                Barcode = "7891000055123",
                Name = "Iogurte Natural Integral",
                Brand = "Nestlé",
                Nutrients = new Nutrients { Calories = 62, Fat = 3.3m, Protein = 3.0m, Sugar = 4.6m, Sodium = 50, Fiber = 0.0m },
                Tags = new[] { "lácteo", "cálcio" },
                Benefits = new[] { "fonte de proteína", "rico em cálcio" },
                Malefits = new[] { "contém lactose", "açúcar adicionado" },
                HealthScore = 82,
                Source = "seed",
                LastUpdated = DateTime.UtcNow
            },
            new()
            {
                Id = "7894900011517",
                Barcode = "7894900011517",
                Name = "Biscoito Recheado Chocolate",
                Brand = "Delícia",
                Nutrients = new Nutrients { Calories = 480, Fat = 20m, Protein = 5m, Sugar = 38m, Sodium = 260, Fiber = 2m, SaturatedFat = 12m },
                Tags = new[] { "biscoito", "lanche" },
                Benefits = new[] { "praticidade" },
                Malefits = new[] { "alto em açúcar", "rico em sódio" },
                HealthScore = 35,
                Source = "seed",
                LastUpdated = DateTime.UtcNow
            },
            new()
            {
                Id = "7891000311304",
                Barcode = "7891000311304",
                Name = "Cereal Integral Aveia e Mel",
                Brand = "GraniMix",
                Nutrients = new Nutrients { Calories = 210, Fat = 4m, Protein = 6m, Sugar = 12m, Sodium = 120, Fiber = 7m },
                Tags = new[] { "cereal", "integral" },
                Benefits = new[] { "rico em fibras", "energia sustentável" },
                Malefits = new[] { "contém glúten" },
                HealthScore = 78,
                Source = "seed",
                LastUpdated = DateTime.UtcNow
            },
            new()
            {
                Id = "7891991010848",
                Barcode = "7891991010848",
                Name = "Refrigerante Cola",
                Brand = "Spark",
                Nutrients = new Nutrients { Calories = 140, Sugar = 39m, Sodium = 45 },
                Tags = new[] { "bebida", "açucarado" },
                Benefits = new[] { "energia rápida" },
                Malefits = new[] { "alto teor de açúcar", "sem valor nutricional" },
                HealthScore = 18,
                Source = "seed",
                LastUpdated = DateTime.UtcNow
            },
            new()
            {
                Id = "7898080641234",
                Barcode = "7898080641234",
                Name = "Mix de Castanhas Premium",
                Brand = "NutriMix",
                Nutrients = new Nutrients { Calories = 160, Fat = 14m, Protein = 6m, Sugar = 2m, Sodium = 2m, Fiber = 3m },
                Tags = new[] { "oleaginosas", "snack saudável" },
                Benefits = new[] { "gorduras boas", "saciedade prolongada" },
                Malefits = new[] { "alto teor calórico" },
                HealthScore = 88,
                Source = "seed",
                LastUpdated = DateTime.UtcNow
            }
        };
    }
}
