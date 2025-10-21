using Npgsql;

namespace NutriScan.Api.Data;

public class SupabaseContext
{
    private readonly IConfiguration _configuration;

    public SupabaseContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public NpgsqlConnection CreateConnection()
    {
        var host = _configuration["SUPABASE_PG_HOST"] ?? throw new InvalidOperationException("SUPABASE_PG_HOST não configurado");
        var database = _configuration["SUPABASE_PG_DB"] ?? "postgres";
        var user = _configuration["SUPABASE_PG_USER"] ?? "postgres";
        var password = _configuration["SUPABASE_PG_PASS"] ?? throw new InvalidOperationException("SUPABASE_PG_PASS não configurado");
        var port = _configuration["SUPABASE_PG_PORT"] ?? "6543";

        var connectionString = $"Host={host};Database={database};Username={user};Password={password};Port={port};SSL Mode=Require;Trust Server Certificate=true";
        return new NpgsqlConnection(connectionString);
    }
}
