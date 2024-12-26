using StackExchange.Redis;
using System.Text.Json;
using TickerAlert.Application.Common.Cache;

namespace TickerAlert.Infrastructure.Cache;

public class RedisCacheService : ICacheService
{
    private readonly IDatabase _redisDatabase;

    public RedisCacheService(IConnectionMultiplexer redisConnection)
    {
        _redisDatabase = redisConnection.GetDatabase();
    }

    public async Task<T?> GetAsync<T>(string namespacePrefix, string key)
    {
        string namespacedKey = CreateNamespacedKey(namespacePrefix, key);
        string? value = await _redisDatabase.StringGetAsync(namespacedKey);

        return !string.IsNullOrEmpty(value) ? JsonSerializer.Deserialize<T>(value) : default;
    }

    public async Task SetAsync(string namespacePrefix, string key, object value, TimeSpan expiration)
    {
        string namespacedKey = CreateNamespacedKey(namespacePrefix, key);
        string serializedValue = JsonSerializer.Serialize(value);

        await _redisDatabase.StringSetAsync(namespacedKey, serializedValue, expiration);
    }

    public async Task<Dictionary<string, T?>> GetMultipleAsync<T>(string namespacePrefix, IEnumerable<string> keys)
    {
        var namespacedKeys = keys
            .Select(key => (RedisKey)CreateNamespacedKey(namespacePrefix, key))
            .ToArray();

        RedisValue[] values = await _redisDatabase.StringGetAsync(namespacedKeys);

        return keys
            .Zip(values, (originalKey, value) => new
            {
                Key = originalKey,
                Value = value.HasValue ? JsonSerializer.Deserialize<T>(value!) : default
            })
            .ToDictionary(x => x.Key, x => x.Value);
    }

    private static string CreateNamespacedKey(string namespacePrefix, string key)
        => $"{namespacePrefix}:{key}";
}
