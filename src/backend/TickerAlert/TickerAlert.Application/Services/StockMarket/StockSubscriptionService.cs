using System.Collections.Concurrent;

namespace TickerAlert.Application.Services.StockMarket;

/// <summary>
/// Servicio de cache en memoria para mantener las subscripciones de los usuarios a los diferentes activos financieros.
/// Utilizado principalmente para notificar a los usuarios mediante SignalR cuando hay nuevas lecturas de precios.
/// </summary>
public sealed class StockSubscriptionService : IStockSubscriptionService
{
    private readonly ConcurrentDictionary<string, HashSet<string>> _subscriptions = new();

    public Task<HashSet<string>> GetUsersSubscribedToStock(string stockSymbol)
    {
        var users = _subscriptions.TryGetValue(stockSymbol, out var userSet)
            ? userSet
            : [];

        return Task.FromResult(users);
    }

    public Task AddSubscription(string stockSymbol, string userId)
    {
        _subscriptions.AddOrUpdate(stockSymbol, [userId], (key, existingUsers) =>
        {
            existingUsers.Add(userId);
            return existingUsers;
        });

        return Task.CompletedTask;
    }

    public Task RemoveSubscription(string stockSymbol, string userId)
    {
        if (_subscriptions.TryGetValue(stockSymbol, out var users))
        {
            users.Remove(userId);
            if (users.Count == 0)
            {
                _subscriptions.TryRemove(stockSymbol, out _);
            }
        }
        return Task.CompletedTask;
    }
}
