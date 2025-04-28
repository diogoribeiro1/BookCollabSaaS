using BookCollabSaaS.Application.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace BookCollabSaaS.Infrastructure.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase _cache;

    public CacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _cache = connectionMultiplexer.GetDatabase();
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var value = await _cache.StringGetAsync(key);
        if (value.IsNullOrEmpty)
            return default;

        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var jsonData = JsonSerializer.Serialize(value);
        await _cache.StringSetAsync(key, jsonData, expiry);
    }

    public async Task RemoveAsync(string key)
    {
        await _cache.KeyDeleteAsync(key);
    }
}