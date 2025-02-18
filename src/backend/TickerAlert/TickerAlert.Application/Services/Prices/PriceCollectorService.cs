using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.Prices.MarketHours;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Application.Services.StockMarket.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Prices;

/// <summary>
/// Main class responsible to read actual prices and save the price measures made.
/// </summary>
public class PriceCollectorService(
    IFinancialAssetReader financialAssetReader,
    IPriceMeasureService priceMeasureService,
    IStockMarketService stockMarketService)
{
    public async Task CollectPrices()
    {
        DateTime now = DateTime.UtcNow;

        // Only for test
        //DateTime fake = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, DateTime.UtcNow.Day, 15, 0, 0, DateTimeKind.Utc); // 3:00 PM UTC

        if (UnitedStatesMarketHours.IsMarketOpen(now))
        {
            await CollectPrices(financialAssetReader, priceMeasureService, stockMarketService);
        }
    }

    private static async Task CollectPrices(IFinancialAssetReader financialAssetReader, IPriceMeasureService priceMeasureService, IStockMarketService stockMarketService)
    {
        List<FinancialAsset> assetsToScan = await CollectFinancialAssetsToScan(financialAssetReader);

        foreach (var asset in assetsToScan)
        {
            var priceMeasureDto = await stockMarketService.ReadPriceFor(asset.Ticker);
            await priceMeasureService.ProcessPriceMeasure(CreatePriceMeasureModel(asset, priceMeasureDto));
        }
    }

    private static async Task<List<FinancialAsset>> CollectFinancialAssetsToScan(IFinancialAssetReader financialAssetReader)
    {
        List<FinancialAsset> assetsInPendingAlerts = await financialAssetReader.GetAllWithPendingAlerts();
        List<FinancialAsset> assetsInWatchlists = await financialAssetReader.GetAllInWatchlists();

        return assetsInPendingAlerts
            .Concat(assetsInWatchlists)
            .DistinctBy(x => x.Ticker)
            .ToList();
    }

    private static PriceMeasure CreatePriceMeasureModel(FinancialAsset asset, PriceMeasureDto priceMeasureDto) 
        => PriceMeasure.Create(Guid.NewGuid(), asset.Id, priceMeasureDto.CurrentPrice);
}