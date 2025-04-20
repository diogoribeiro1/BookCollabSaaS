using System;
using BookCollabSaaS.Application.Interfaces;
using StackExchange.Redis;

namespace BookCollabSaaS.Infrastructure.Services;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _database;

    public RedisCacheService(IConnectionMultiplexer connectionMultiplexer)
    {
        _database = connectionMultiplexer.GetDatabase();
    }

    public async Task SetAsync(string key, string value)
    {
        await _database.StringSetAsync(key, value);
    }

    public async Task<string?> GetAsync(string key)
    {
        return await _database.StringGetAsync(key);
    }
}