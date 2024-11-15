using MediatR;
using TickerAlert.Application.Interfaces.Watchlists;
using TickerAlert.Application.Interfaces.Watchlists.Dtos;

namespace TickerAlert.Application.UseCases.Watchlists.Queries.GetUserWatchlist;

public sealed record GetUserWatchlistRequest : IRequest<WatchlistDto>;

public sealed class GetUserWatchlistRequestHandler(IWatchlistService service) 
    : IRequestHandler<GetUserWatchlistRequest, WatchlistDto>
{
    public async Task<WatchlistDto> Handle(GetUserWatchlistRequest request, CancellationToken cancellationToken) 
        => await service.GetWatchlist();
}
