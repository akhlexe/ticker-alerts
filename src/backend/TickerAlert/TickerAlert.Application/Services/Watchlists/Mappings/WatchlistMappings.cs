using TickerAlert.Application.Interfaces.Watchlists.Dtos;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Watchlists.Mappings;

public static class WatchlistMappings
{
    private const string TickernameNotFound = "Tickername not found";

    public static WatchlistDto MapToDto(
        Watchlist watchlist, 
        Dictionary<Guid, FinancialAssetDto> assets, 
        Dictionary<Guid, PriceMeasure> lastPrices, 
        Dictionary<Guid, PriceMeasure> yesterdayPrices)
    {
        return new WatchlistDto
        {
            Id = watchlist.Id,
            Name = watchlist.Name,
            UserId = watchlist.UserId,
            Items = watchlist.WatchlistItems.Select(item => MapToDto(
                item, 
                assets.GetValueOrDefault(item.FinancialAssetId),
                lastPrices.GetValueOrDefault(item.FinancialAssetId),
                yesterdayPrices.GetValueOrDefault(item.FinancialAssetId))
            ).ToList()
        };
    }

    private static WatchlistItemDto MapToDto(WatchlistItem item, FinancialAssetDto? asset, PriceMeasure? lastPrice, PriceMeasure? yesterdayPrice)
    {
        return new WatchlistItemDto
        {
            Id = item.Id,
            FinancialAssetId = item.FinancialAssetId,
            WatchlistId = item.WatchlistId,
            TickerName = asset?.Ticker ?? TickernameNotFound,
            Price = lastPrice?.Price ?? 0,
            Variation = CalculateVariation(lastPrice, yesterdayPrice),
        };
    }

    private static decimal CalculateVariation(PriceMeasure? lastPrice, PriceMeasure? yesterdayPrice)
    {
        decimal last = lastPrice?.Price ?? 0;
        decimal yesterday = yesterdayPrice?.Price ?? 0;

        if (yesterday == 0)
        {
            return 0;
        }

        return  (last / yesterday) - 1;
    }
}
