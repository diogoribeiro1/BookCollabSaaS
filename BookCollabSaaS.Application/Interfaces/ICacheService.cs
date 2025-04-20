using System;

namespace BookCollabSaaS.Application.Interfaces;

public interface ICacheService
{
    Task SetAsync(string key, string value);
    Task<string?> GetAsync(string key);
}
