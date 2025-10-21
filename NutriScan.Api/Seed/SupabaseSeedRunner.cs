using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NutriScan.Api.Data;

namespace NutriScan.Api.Seed;

public static class SupabaseSeedRunner
{
    public static async Task RunAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<SupabaseContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILoggerFactory>().CreateLogger("SupabaseSeed");
        var sqlPath = Path.Combine(AppContext.BaseDirectory, "Seed", "SupabaseSeed.sql");

        if (!File.Exists(sqlPath))
        {
            logger.LogWarning("Arquivo de seed Supabase não encontrado em {Path}", sqlPath);
            return;
        }

        var script = await File.ReadAllTextAsync(sqlPath);

        await using var connection = context.CreateConnection();
        await connection.OpenAsync();
        await using var command = connection.CreateCommand();
        command.CommandText = script;
        await command.ExecuteNonQueryAsync();

        logger.LogInformation("Seed Supabase executado com sucesso");
    }
}
