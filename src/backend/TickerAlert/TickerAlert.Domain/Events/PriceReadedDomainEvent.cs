using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Events;

public record PriceReadedDomainEvent(Guid Id, int PriceMeasureId) : IDomainEvent;