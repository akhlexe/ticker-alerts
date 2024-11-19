using MediatR;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.Dtos;

namespace TickerAlert.Application.UseCases.FinancialAssets.Queries.SearchFinancialAsset;

public record SearchFinancialAssetRequest(string SearchString) : IRequest<IEnumerable<FinancialAssetDto>>;

public class SearchFinancialAssetRequestHandler(IFinancialAssetReader financialAssetReader) 
    : IRequestHandler<SearchFinancialAssetRequest, IEnumerable<FinancialAssetDto>>
{
    public async Task<IEnumerable<FinancialAssetDto>> Handle(SearchFinancialAssetRequest request, CancellationToken cancellationToken) 
        => await financialAssetReader.GetAllBySearchCriteria(request.SearchString);
}