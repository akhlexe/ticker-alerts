using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Interfaces.PriceMeasures;
using TickerAlert.Application.Services.StockMarket;
using TickerAlert.Application.Services.StockMarket.Dtos;
using TickerAlert.Domain.Entities;

namespace TickerAlert.Application.Services.PriceReader;

/// <summary>
/// Main class responsible to read actual prices and save the price measures made.
/// </summary>
public class PriceReaderService
{
    private readonly IFinancialAssetRepository _financialAssetRepository;
    private readonly IPriceMeasureRepository _priceMeasureRepository;
    private readonly IStockMarketService _stockMarketService;

    public PriceReaderService(
        IFinancialAssetRepository financialAssetRepository,
        IPriceMeasureRepository priceMeasureRepository,
        IStockMarketService stockMarketService)
    {
        _financialAssetRepository = financialAssetRepository;
        _priceMeasureRepository = priceMeasureRepository;
        _stockMarketService = stockMarketService;
    }

    public async Task ReadPricesAndSaveAsync()
    {
        var pendingAssets = await _financialAssetRepository.GetAllWithPendingAlerts();

        foreach (var asset in pendingAssets)
        {
            var priceMeasureDto = await _stockMarketService.ReadPriceFor(asset.Ticker);
            await _priceMeasureRepository.RegisterPriceMeasure(CreatePriceMeasureModel(asset, priceMeasureDto));
        }
    }

    private static PriceMeasure CreatePriceMeasureModel(FinancialAsset asset, PriceMeasureDto priceMeasureDto)
    {
        return new PriceMeasure(asset.Id, priceMeasureDto.CurrentPrice, DateTime.UtcNow);
    }
}