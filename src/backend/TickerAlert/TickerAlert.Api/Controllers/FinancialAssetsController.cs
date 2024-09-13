using Microsoft.AspNetCore.Mvc;
using TickerAlert.Application.Common.Responses;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Application.Services.StockMarket.Dtos;
using TickerAlert.Application.UseCases.FinancialAssets.Queries.GetFinancialAsset;
using TickerAlert.Application.UseCases.FinancialAssets.Queries.GetFinancialAssetProfile;
using TickerAlert.Application.UseCases.FinancialAssets.Queries.SearchFinancialAsset;

namespace TickerAlert.Api.Controllers;

public class FinancialAssetsController : ApiController
{
    [HttpGet("{id}")]
    public async Task<Result<FinancialAssetDto>> GetFinancialAsset([FromRoute] GetFinancialAssetRequest query)
        => await Mediator.Send(query);

    [HttpGet]
    public async Task<IEnumerable<FinancialAssetDto>> GetFinancialAssets([FromQuery] string criteria) 
        => await Mediator.Send(new SearchFinancialAssetRequest(criteria));

    [HttpGet("Profile")]
    public async Task<CompanyProfileDto> GetFinancialAssetProfile([FromQuery] GetFinancialAssetProfileRequest query) 
        => await Mediator.Send(query);
}