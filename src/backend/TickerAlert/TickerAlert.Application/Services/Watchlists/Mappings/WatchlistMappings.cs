using TickerAlert.Application.Interfaces.Watchlists.Dtos;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Watchlists.Mappings;

public static class WatchlistMappings
{
    private const string TickernameNotFound = "Tickername not found";

    public static WatchlistDto MapToDto(Watchlist watchlist, Dictionary<Guid, FinancialAssetDto> assets)
    {
        return new WatchlistDto
        {
            Id = watchlist.Id,
            Name = watchlist.Name,
            UserId = watchlist.UserId,
            WatchlistItems = watchlist.WatchlistItems.Select(item => MapToDto(item, assets.GetValueOrDefault(item.FinancialAssetId))).ToList()
        };
    }

    private static WatchlistItemDto MapToDto(WatchlistItem item, FinancialAssetDto? asset)
    {
        return new WatchlistItemDto
        {
            Id = item.Id,
            FinancialAssetId = item.FinancialAssetId,
            WatchlistId = item.WatchlistId,
            TickerName = asset?.Ticker ?? TickernameNotFound
        };
    }
}
