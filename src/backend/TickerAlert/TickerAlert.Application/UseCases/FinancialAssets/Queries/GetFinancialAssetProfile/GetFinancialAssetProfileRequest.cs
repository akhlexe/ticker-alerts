using MediatR;
using TickerAlert.Application.Services.FinancialAssets;
using TickerAlert.Application.Services.StockMarket.Dtos;

namespace TickerAlert.Application.UseCases.FinancialAssets.Queries.GetFinancialAssetProfile;

public sealed record GetFinancialAssetProfileRequest(Guid FinancialAssetId) : IRequest<CompanyProfileDto>;

public sealed class GetFinancialAssetProfileRequestHandler(FinancialAssetProfileService financialAssetProfileService) 
    : IRequestHandler<GetFinancialAssetProfileRequest, CompanyProfileDto>
{
    public async Task<CompanyProfileDto> Handle(GetFinancialAssetProfileRequest request, CancellationToken cancellationToken) 
        => await financialAssetProfileService.GetFinancialAssetProfileAsync(request.FinancialAssetId);
}
