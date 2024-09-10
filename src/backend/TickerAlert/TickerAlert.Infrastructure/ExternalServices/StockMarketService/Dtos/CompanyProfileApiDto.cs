using TickerAlert.Application.Services.StockMarket.Dtos;

namespace TickerAlert.Infrastructure.ExternalServices.StockMarketService.Dtos;

public sealed class CompanyProfileApiDto
{
    //[JsonPropertyName("country")]
    public string Country { get; set; }
    public string Currency { get; set; }
    public string Exchange { get; set; }
    public DateTime Ipo { get; set; }
    public decimal MarketCapitalization { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }
    public decimal ShareOutstanding { get; set; }
    public string Ticker { get; set; }
    public string Weburl { get; set; }
    public string Logo { get; set; }
    public string FinnhubIndustry { get; set; }
}

public static class CompanyProfileMapper
{
    public static CompanyProfileDto ToCompanyProfileDto(CompanyProfileApiDto companyProfile)
    {
        return new CompanyProfileDto
        {
            Ticker = companyProfile.Ticker,
            Name = companyProfile.Name,
            Logo = companyProfile.Logo,
            Country = companyProfile.Country,
            Currency = companyProfile.Currency,
            Exchange = companyProfile.Exchange,
            FinnhubIndustry = companyProfile.FinnhubIndustry,
            Ipo = companyProfile.Ipo.ToShortDateString(),
            MarketCapitalization = companyProfile.MarketCapitalization,
            Phone = companyProfile.Phone,
            ShareOutstanding = companyProfile.ShareOutstanding,
            Weburl = companyProfile.Weburl
        };
    }

    public static CompanyProfileDto CreateNullCompanyProfile(string ticker)
    {
        return new CompanyProfileDto
        {
            Ticker = ticker,
            Name = "N/A",
            Logo = "N/A",
            Country = "N/A",
            Currency = "N/A",
            Exchange = "N/A",
            FinnhubIndustry = "N/A",
            Ipo = "N/A",
            MarketCapitalization = 0,
            Phone = "N/A",
            ShareOutstanding = 0,
            Weburl = "N/A"
        };
    }
}