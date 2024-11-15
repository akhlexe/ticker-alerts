using MediatR;
using TickerAlert.Application.Interfaces.Watchlists;
using TickerAlert.Application.Interfaces.Watchlists.Dtos;

namespace TickerAlert.Application.UseCases.Watchlists.Commands.AddWatchlistItem;

public sealed record AddWatchlistItemRequest(Guid WatchlistId, Guid FinancialAssetId) : IRequest<WatchlistDto>;

public sealed class AddWatchlistItemRequestHandler(IWatchlistService service) 
    : IRequestHandler<AddWatchlistItemRequest, WatchlistDto>
{
    public Task<WatchlistDto> Handle(AddWatchlistItemRequest request, CancellationToken cancellationToken) 
        => service.AddItem(request.WatchlistId, request.FinancialAssetId);
};