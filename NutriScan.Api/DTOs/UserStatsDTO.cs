namespace NutriScan.Api.DTOs;

public record UserStatsDTO(
    IDictionary<string, decimal> Today,
    IEnumerable<DailyCalories> WeeklyCalories
);

public record DailyCalories(string Day, decimal Calories);
