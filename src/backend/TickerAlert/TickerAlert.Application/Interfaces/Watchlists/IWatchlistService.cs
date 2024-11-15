using TickerAlert.Application.Interfaces.Watchlists.Dtos;

namespace TickerAlert.Application.Interfaces.Watchlists;

public interface IWatchlistService
{
    Task<WatchlistDto> GetWatchlist();
    Task<WatchlistDto> AddItem(Guid watchlistId, Guid financialAssetId);
    Task<WatchlistDto> RemoveItem(Guid watchlistId, Guid itemId);
}
