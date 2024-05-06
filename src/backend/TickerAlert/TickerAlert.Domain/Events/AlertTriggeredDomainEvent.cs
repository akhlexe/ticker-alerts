using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Events;

public record AlertTriggeredDomainEvent(Guid Id, int AlertId) : IDomainEvent;