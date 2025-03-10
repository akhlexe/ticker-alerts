﻿using TickerAlert.Application.Interfaces.Watchlists.Dtos;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Watchlists.Mappings;

public static class WatchlistMappings
{
    private const string TickernameNotFound = "Tickername not found";

    // TODO: improve this adding exchange and ticker when exchange info is available
    // https://www.tradingview.com/chart/siNTkFlw/?symbol=<exchange>%3A<ticker>
    private const string ChartUrl = "https://www.tradingview.com/chart/siNTkFlw/?symbol=";

    public static WatchlistDto MapToDto(
        Watchlist watchlist, 
        Dictionary<Guid, FinancialAssetDto> assets, 
        Dictionary<Guid, decimal> lastPrices)
    {
        return new WatchlistDto
        {
            Id = watchlist.Id,
            Name = watchlist.Name,
            UserId = watchlist.UserId,
            Items = watchlist.WatchlistItems.Select(item => MapToDto(
                item, 
                assets.GetValueOrDefault(item.FinancialAssetId),
                lastPrices.GetValueOrDefault(item.FinancialAssetId))
            ).ToList()
        };
    }

    private static WatchlistItemDto MapToDto(WatchlistItem item, FinancialAssetDto? asset, decimal lastPrice)
    {
        return new WatchlistItemDto
        {
            Id = item.Id,
            FinancialAssetId = item.FinancialAssetId,
            WatchlistId = item.WatchlistId,
            TickerName = asset?.Ticker ?? TickernameNotFound,
            Price = lastPrice,
            ChartLink = ChartUrl + asset?.Ticker
        };
    }
}
