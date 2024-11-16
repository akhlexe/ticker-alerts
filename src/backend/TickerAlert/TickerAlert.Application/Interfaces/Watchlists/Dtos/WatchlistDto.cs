namespace TickerAlert.Application.Interfaces.Watchlists.Dtos;

public class WatchlistDto
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public string Name { get; set; }

    public List<WatchlistItemDto> Items { get; set; }
}