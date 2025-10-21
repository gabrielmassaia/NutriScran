using NutriScan.Api.Data;
using NutriScan.Api.Seed;
using NutriScan.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddSingleton<MongoContext>();
builder.Services.AddSingleton<SupabaseContext>();

builder.Services.AddHttpClient<OpenFoodFactsService>(client =>
{
    var baseUrl = builder.Configuration["OPENFOODFACTS_API_BASE"] ?? "https://world.openfoodfacts.org/api/v2/product/";
    client.BaseAddress = new Uri(baseUrl);
    client.Timeout = TimeSpan.FromSeconds(10);
});

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SupabaseService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

if (bool.TryParse(builder.Configuration["SEED"], out var shouldSeed) && shouldSeed)
{
    try
    {
        await MongoSeed.RunAsync(app.Services);
        await SupabaseSeedRunner.RunAsync(app.Services);
    }
    catch (Exception ex)
    {
        app.Logger.LogWarning(ex, "Seed executado com erros não críticos");
    }
}

await app.RunAsync();
