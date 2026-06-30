using System.Collections.Concurrent;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Repository;
using Repository.Constants;

namespace Service;


public class BusinessRuleService(IServiceScopeFactory scopeFactory)
{
    private readonly ConcurrentDictionary<BusinessRuleConstantType, string> _cache = new();

    public T GetValue<T>(BusinessRuleConstantType type)
    {
        var raw = _cache.GetOrAdd(type, LoadFromDb);
        return (T)Convert.ChangeType(raw, typeof(T), CultureInfo.InvariantCulture);
    }

    public async Task SetValueAsync(BusinessRuleConstantType type, object value)
    {
        var raw = Convert.ToString(value, CultureInfo.InvariantCulture)
                  ?? throw new ArgumentException("Không thể chuyển đổi giá trị sang chuỗi.", nameof(value));

        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var rule = await context.BusinessRules.FindAsync(type)
                   ?? throw new InvalidOperationException($"Không tìm thấy business rule: {type}");

        rule.Value = raw;
        await context.SaveChangesAsync();

        _cache[type] = raw;
    }

    public long CalculateLoyaltyPoints(long totalAmount)
    {
        var vndPerPoint = GetValue<int>(BusinessRuleConstantType.VNDPerLoyaltyPoint);
        return Math.Max(0, totalAmount / vndPerPoint);
    }

    private string LoadFromDb(BusinessRuleConstantType type)
    {
        using var scope = scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var rule = context.BusinessRules
            .AsNoTracking()
            .FirstOrDefault(br => br.Type == type);

        return rule?.Value
               ?? throw new InvalidOperationException($"Không tìm thấy business rule trong DB: {type}");
    }
}
