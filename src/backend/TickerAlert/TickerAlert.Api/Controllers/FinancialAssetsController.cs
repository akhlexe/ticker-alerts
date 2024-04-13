using Microsoft.AspNetCore.Mvc;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Application.UseCases.FinancialAssets.Queries.SearchFinancialAsset;

namespace TickerAlert.Api.Controllers;

public class FinancialAssetsController : ApiController
{
    [HttpGet]
    public async Task<IEnumerable<FinancialAssetDto>> GetFinancialAssets([FromQuery] string criteria) 
        => await Mediator.Send(new SearchFinancialAssetRequest(criteria));
}