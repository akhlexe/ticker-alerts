using TickerAlert.Application.Interfaces.Cedears.Dtos;
using TickerAlert.Application.Services.StockMarket.Dtos;

namespace TickerAlert.Application.Services.FinancialAssets.Dtos;

public class FinancialAssetProfileDto
{
    public CompanyProfileDto Profile { get; set; }
    public CedearCotizacion CedearCotizacion { get; set; }
}
