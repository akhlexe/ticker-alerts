using MediatR;
using TickerAlert.Application.Common.Responses;
using TickerAlert.Application.Interfaces.FinancialAssets;
using TickerAlert.Application.Services.FinancialAssets.Dtos;

namespace TickerAlert.Application.UseCases.FinancialAssets.Queries.GetFinancialAsset;

public sealed record GetFinancialAssetRequest(Guid Id) : IRequest<Result<FinancialAssetDto>>;

public sealed class GetFinancialAssetRequestHandler(IFinancialAssetReader financialAssetReader) 
    : IRequestHandler<GetFinancialAssetRequest, Result<FinancialAssetDto>>
{
    public async Task<Result<FinancialAssetDto>> Handle(GetFinancialAssetRequest request, CancellationToken cancellationToken) 
        => await financialAssetReader.GetById(request.Id);
}
