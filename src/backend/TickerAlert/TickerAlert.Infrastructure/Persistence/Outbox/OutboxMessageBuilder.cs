using Newtonsoft.Json;
using TickerAlert.Domain.Common;

namespace TickerAlert.Infrastructure.Persistence.Outbox;

public static class OutboxMessageBuilder
{
    private static readonly JsonSerializerSettings SerializerSettings = new()
    {
        TypeNameHandling = TypeNameHandling.All
    };
    
    public static OutboxMessage CreateOutboxMessage(IDomainEvent domainEvent)
    {
        return new OutboxMessage()
        {
            Id = Guid.NewGuid(),
            Name = domainEvent.GetType().Name, 
            Content = JsonConvert.SerializeObject(domainEvent, SerializerSettings),
            CreatedOnUtc = DateTime.UtcNow
        };
    }
}