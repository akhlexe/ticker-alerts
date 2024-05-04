namespace TickerAlert.Infrastructure.Persistence.Outbox;

public sealed class OutboxMessage
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Content { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? ProcessedOnUtc { get; set; } = null;
    public string? Error { get; set; } = null;
}
    
