using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Interfaces.PriceMeasures;
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
        var pendingAssets = await financialAssetReader.GetAllWithPendingAlerts();

        foreach (var asset in pendingAssets)
        {
            var priceMeasureDto = await stockMarketService.ReadPriceFor(asset.Ticker);
            await priceMeasureService.ProcessPriceMeasure(CreatePriceMeasureModel(asset, priceMeasureDto));
        }
    }

    private static PriceMeasure CreatePriceMeasureModel(FinancialAsset asset, PriceMeasureDto priceMeasureDto) 
        => PriceMeasure.Create(Guid.NewGuid(), asset.Id, priceMeasureDto.CurrentPrice);
}