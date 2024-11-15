using MediatR;
using TickerAlert.Application.Interfaces.Watchlists;
using TickerAlert.Application.Interfaces.Watchlists.Dtos;

namespace TickerAlert.Application.UseCases.Watchlists.Commands.RemoveWatchlistItem;

public sealed record RemoveWatchlistItemRequest(Guid WatchlistId, Guid ItemId) : IRequest<WatchlistDto>;

public sealed class RemoveWatchlistItemRequestHandler(IWatchlistService service) 
    : IRequestHandler<RemoveWatchlistItemRequest, WatchlistDto>
{
    public async Task<WatchlistDto> Handle(RemoveWatchlistItemRequest request, CancellationToken cancellationToken) 
        => await service.RemoveItem(request.WatchlistId, request.ItemId);
}
