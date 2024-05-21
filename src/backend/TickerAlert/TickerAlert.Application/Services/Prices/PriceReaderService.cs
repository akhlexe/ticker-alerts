using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Application.Services.StockMarket.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.Prices;

/// <summary>
/// Main class responsible to read actual prices and save the price measures made.
/// </summary>
public class PriceReaderService
{
    private readonly IFinancialAssetReader _financialAssetReader;
    private readonly IPriceMeasureService _priceMeasureReader;
    private readonly IStockMarketService _stockMarketService;

    public PriceReaderService(
        IFinancialAssetReader financialAssetReader,
        IPriceMeasureService priceMeasureService,
        IStockMarketService stockMarketService)
    {
        _financialAssetReader = financialAssetReader;
        _priceMeasureReader = priceMeasureService;
        _stockMarketService = stockMarketService;
    }

    public async Task ReadPricesAndSaveAsync()
    {
        var pendingAssets = await _financialAssetReader.GetAllWithPendingAlerts();

        foreach (var asset in pendingAssets)
        {
            var priceMeasureDto = await _stockMarketService.ReadPriceFor(asset.Ticker);
            await _priceMeasureReader.RegisterPriceMeasure(CreatePriceMeasureModel(asset, priceMeasureDto));
        }
    }

    private static PriceMeasure CreatePriceMeasureModel(FinancialAsset asset, PriceMeasureDto priceMeasureDto) 
        => PriceMeasure.Create(Guid.NewGuid(), asset.Id, priceMeasureDto.CurrentPrice);
}