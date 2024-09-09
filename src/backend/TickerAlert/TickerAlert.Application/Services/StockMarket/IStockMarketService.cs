using TickerAlert.Application.Services.StockMarket.Dtos;

namespace TickerAlert.Application.Services.StockMarket;

public interface IStockMarketService
{
    Task<PriceMeasureDto> ReadPriceFor(string ticker);
    Task<CompanyProfileDto> GetCompanyProfile(string ticker);
}