using Microsoft.Extensions.Options;
using MongoDB.Driver;
using NutriScan.Api.Models;

namespace NutriScan.Api.Data;

public class MongoContext
{
    private readonly IMongoDatabase _database;
    private readonly string _collectionName;

    public MongoContext(IConfiguration configuration)
    {
        var connectionString = configuration["MONGODB_URI"] ?? throw new InvalidOperationException("MONGODB_URI não configurado");
        var databaseName = configuration["MONGODB_DB"] ?? "nutriscan";
        _collectionName = configuration["MONGODB_COLLECTION"] ?? "products";

        var client = new MongoClient(connectionString);
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Product> Products => _database.GetCollection<Product>(_collectionName);
}
