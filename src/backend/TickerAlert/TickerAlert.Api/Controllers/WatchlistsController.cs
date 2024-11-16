using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TickerAlert.Application.Interfaces.Watchlists.Dtos;
using TickerAlert.Application.UseCases.Watchlists.Commands.AddWatchlistItem;
using TickerAlert.Application.UseCases.Watchlists.Commands.RemoveWatchlistItem;
using TickerAlert.Application.UseCases.Watchlists.Queries.GetUserWatchlist;

namespace TickerAlert.Api.Controllers;

[Authorize]
public class WatchlistsController : ApiController
{
    [HttpGet]
    public async Task<WatchlistDto> GetUserWatchlist() 
        => await Mediator.Send(new GetUserWatchlistRequest());

    [HttpPost("AddItem")]
    public Task<WatchlistDto> AddItem([FromBody] AddWatchlistItemRequest request) 
        => Mediator.Send(request);

    [HttpDelete("RemoveItem")]
    public Task<WatchlistDto> RemoveItem([FromBody] RemoveWatchlistItemRequest request) 
        => Mediator.Send(request);
}
