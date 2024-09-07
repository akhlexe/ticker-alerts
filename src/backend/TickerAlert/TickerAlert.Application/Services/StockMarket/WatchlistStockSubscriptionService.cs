using System.Collections.Concurrent;

namespace TickerAlert.Application.Services.StockMarket;

/// <summary>
/// Servicio de cache en memoria para mantener las subscripciones de los usuarios a los diferentes activos financieros.
/// Utilizado principalmente para notificar a los usuarios mediante SignalR cuando hay nuevas lecturas de precios.
/// </summary>
public sealed class WatchlistStockSubscriptionService : IWatchlistStockSubscriptionService
{
    private readonly ConcurrentDictionary<Guid, HashSet<string>> _subscriptions = new();

    public Task<HashSet<string>> GetUsersSubscribedToStock(Guid assetId)
    {
        var users = _subscriptions.TryGetValue(assetId, out var userSet)
            ? userSet
            : [];

        return Task.FromResult(users);
    }

    public Task AddSubscription(Guid assetId, string userId)
    {
        _subscriptions.AddOrUpdate(assetId, [userId], (key, existingUsers) =>
        {
            existingUsers.Add(userId);
            return existingUsers;
        });

        return Task.CompletedTask;
    }

    public Task RemoveSubscription(Guid assetId, string userId)
    {
        if (_subscriptions.TryGetValue(assetId, out var users))
        {
            users.Remove(userId);
            if (users.Count == 0)
            {
                _subscriptions.TryRemove(assetId, out _);
            }
        }
        return Task.CompletedTask;
    }
}
