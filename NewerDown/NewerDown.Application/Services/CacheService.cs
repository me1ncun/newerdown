using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using NewerDown.Application.Constants;
using NewerDown.Domain.Interfaces;

namespace NewerDown.Application.Services;

public class CacheService : ICacheService
{
    private readonly IDistributedCache _cache;
    private readonly JsonSerializerOptions _jsonSerializerSettings = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = true
    };
    
    public CacheService(IDistributedCache cache)
    {
        _cache = cache;
    }
    
    public async Task SetAsync<T>(string key, T value, TimeSpan? duration = null)
    {
        var json = JsonSerializer.Serialize(value, _jsonSerializerSettings);

        await _cache.SetStringAsync(key, json, new DistributedCacheEntryOptions().SetAbsoluteExpiration(duration ?? TimeSpan.FromMinutes(CacheConstants.DefaultCacheDurationInMinutes)));
    }

    public async Task<T> GetAsync<T>(string key)
    {
        var json = await _cache.GetStringAsync(key);

        if (!string.IsNullOrEmpty(json))
        {
            return JsonSerializer.Deserialize<T>(json, _jsonSerializerSettings);
        }

        return default(T);
    }
    
    public async Task RemoveAsync(string key)
    {
        await _cache.RemoveAsync(key);
    }
}