namespace TickerAlert.Application.Interfaces.Watchlists.Dtos;

public class WatchlistItemDto
{
    public Guid Id { get; set; }

    public Guid FinancialAssetId { get; set; }

    public string TickerName { get; set; }

    public Guid WatchlistId { get; set; }
}
