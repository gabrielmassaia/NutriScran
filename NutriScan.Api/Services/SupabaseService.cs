using NutriScan.Api.Data;
using NutriScan.Api.DTOs;
using NutriScan.Api.Models;
using Npgsql;

namespace NutriScan.Api.Services;

public class SupabaseService
{
    private readonly SupabaseContext _context;
    private readonly ILogger<SupabaseService> _logger;

    public SupabaseService(SupabaseContext context, ILogger<SupabaseService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task InsertLogAsync(Guid userId, UserLogDTO dto)
    {
        await using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        const string commandText = @"
            INSERT INTO public.user_logs (user_id, barcode, quantity, meal)
            VALUES (@user_id, @barcode, @quantity, @meal);
        ";

        await using var command = new NpgsqlCommand(commandText, connection);
        command.Parameters.AddWithValue("user_id", userId);
        command.Parameters.AddWithValue("barcode", dto.Barcode);
        command.Parameters.AddWithValue("quantity", dto.Quantity);
        command.Parameters.AddWithValue("meal", dto.Meal);

        await command.ExecuteNonQueryAsync();
        _logger.LogInformation("Log inserido para usuário {UserId} e produto {Barcode}", userId, dto.Barcode);
    }

    public async Task<IReadOnlyCollection<UserLog>> GetLogsAsync(Guid userId)
    {
        await using var connection = _context.CreateConnection();
        await connection.OpenAsync();

        const string query = @"
            SELECT id, user_id, barcode, scanned_at, quantity, meal
            FROM public.user_logs
            WHERE user_id = @user_id
            ORDER BY scanned_at DESC;
        ";

        await using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("user_id", userId);

        var logs = new List<UserLog>();
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            logs.Add(new UserLog
            {
                Id = reader.GetGuid(0),
                UserId = reader.GetGuid(1),
                Barcode = reader.GetString(2),
                ScannedAt = reader.GetDateTime(3),
                Quantity = reader.GetDecimal(4),
                Meal = reader.GetString(5)
            });
        }

        return logs;
    }
}
