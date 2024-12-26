﻿using TickerAlert.Application.Interfaces.Watchlists.Dtos;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Watchlists.Mappings;

public static class WatchlistMappings
{
    private const string TickernameNotFound = "Tickername not found";

    public static WatchlistDto MapToDto(
        Watchlist watchlist, 
        Dictionary<Guid, FinancialAssetDto> assets, 
        Dictionary<Guid, decimal> lastPrices, 
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

    private static WatchlistItemDto MapToDto(WatchlistItem item, FinancialAssetDto? asset, decimal lastPrice, PriceMeasure? yesterdayPrice)
    {
        return new WatchlistItemDto
        {
            Id = item.Id,
            FinancialAssetId = item.FinancialAssetId,
            WatchlistId = item.WatchlistId,
            TickerName = asset?.Ticker ?? TickernameNotFound,
            Price = lastPrice,
            Variation = CalculateVariation(lastPrice, yesterdayPrice),
        };
    }

    private static decimal CalculateVariation(decimal lastPrice, PriceMeasure? yesterdayPrice)
    {
        decimal yesterday = yesterdayPrice?.Price ?? 0;

        if (yesterday == 0)
        {
            return 0;
        }

        return  (lastPrice / yesterday) - 1;
    }
}
