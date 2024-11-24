namespace TickerAlert.Application.Common.Cache;

public interface ICacheService
{
    Task<T?> GetAsync<T>(string namespacePrefix, string key);
    Task SetAsync(string namespacePrefix, string key, object value, TimeSpan expiration);
    Task<Dictionary<string, T?>> GetMultipleAsync<T>(string namespacePrefix, IEnumerable<string> keys);
}
