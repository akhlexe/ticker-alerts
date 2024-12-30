using TickerAlert.Application.Services.StockMarket.Dtos;

namespace TickerAlert.Application.Services.FinancialAssets.Dtos;

public class FinancialAssetProfileDto
{
    public CompanyProfileDto Profile { get; set; }
    public CedearInformationDto CedearInformation { get; set; }
}
