using MongoDB.Driver;
using NutriScan.Api.Data;
using NutriScan.Api.Models;

namespace NutriScan.Api.Services;

public class ProductService
{
    private readonly MongoContext _mongoContext;
    private readonly OpenFoodFactsService _openFoodFactsService;
    private readonly ILogger<ProductService> _logger;

    public ProductService(MongoContext mongoContext, OpenFoodFactsService openFoodFactsService, ILogger<ProductService> logger)
    {
        _mongoContext = mongoContext;
        _openFoodFactsService = openFoodFactsService;
        _logger = logger;
    }

    public async Task<Product?> GetProductAsync(string barcode)
    {
        var collection = _mongoContext.Products;
        var existing = await collection.Find(p => p.Barcode == barcode).FirstOrDefaultAsync();
        if (existing != null)
        {
            return existing;
        }

        var fetched = await _openFoodFactsService.FetchProductAsync(barcode);
        if (fetched == null)
        {
            _logger.LogInformation("Produto {Barcode} não encontrado no OpenFoodFacts", barcode);
            return null;
        }

        await collection.ReplaceOneAsync(p => p.Id == fetched.Id, fetched, new ReplaceOptions { IsUpsert = true });
        _logger.LogInformation("Produto {Barcode} armazenado após fallback OpenFoodFacts", barcode);
        return fetched;
    }

    public async Task<Product> CreateProductAsync(Product product)
    {
        product.Id = string.IsNullOrWhiteSpace(product.Id) ? product.Barcode : product.Id;
        product.LastUpdated = DateTime.UtcNow;
        product.HealthScore = HealthScoreCalculator.Calculate(product.Nutrients);
        await _mongoContext.Products.ReplaceOneAsync(p => p.Id == product.Id, product, new ReplaceOptions { IsUpsert = true });
        return product;
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _mongoContext.Products.Find(FilterDefinition<Product>.Empty).ToListAsync();
    }
}
