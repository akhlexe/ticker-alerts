using MediatR;
using TickerAlert.Application.Services.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.Dtos;
using TickerAlert.Application.Services.StockMarket.Dtos;

namespace TickerAlert.Application.UseCases.FinancialAssets.Queries.GetFinancialAssetProfile;

public sealed record GetFinancialAssetProfileRequest(Guid FinancialAssetId) : IRequest<FinancialAssetProfileDto>;

public sealed class GetFinancialAssetProfileRequestHandler(FinancialAssetProfileService financialAssetProfileService) 
    : IRequestHandler<GetFinancialAssetProfileRequest, FinancialAssetProfileDto>
{
    public async Task<FinancialAssetProfileDto> Handle(GetFinancialAssetProfileRequest request, CancellationToken cancellationToken) 
        => await financialAssetProfileService.GetFinancialAssetProfileAsync(request.FinancialAssetId);
}
