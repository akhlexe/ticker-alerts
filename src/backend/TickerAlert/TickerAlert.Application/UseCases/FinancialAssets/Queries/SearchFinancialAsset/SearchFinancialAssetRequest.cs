using MediatR;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.Dtos;

namespace TickerAlert.Application.UseCases.FinancialAssets.Queries.SearchFinancialAsset;

public record SearchFinancialAssetRequest(string SearchString) : IRequest<IEnumerable<FinancialAssetDto>>;

public class SearchFinancialAssetRequestHandler : IRequestHandler<SearchFinancialAssetRequest, IEnumerable<FinancialAssetDto>>
{
    private readonly IFinancialAssetReader _financialAssetReader;

    public SearchFinancialAssetRequestHandler(IFinancialAssetReader financialAssetReader) 
        => _financialAssetReader = financialAssetReader;

    public async Task<IEnumerable<FinancialAssetDto>> Handle(SearchFinancialAssetRequest request, CancellationToken cancellationToken) 
        => await _financialAssetReader.GetAllBySearchCriteria(request.SearchString);
}