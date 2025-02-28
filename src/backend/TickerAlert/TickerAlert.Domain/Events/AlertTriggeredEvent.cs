using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Events;

public record AlertTriggeredEvent(Guid Id, Guid AlertId) : IDomainEvent;