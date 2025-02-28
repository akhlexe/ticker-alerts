using TickerAlert.Domain.Common;

namespace TickerAlert.Domain.Events;

public record PriceUpdateEvent(Guid Id, Guid PriceMeasureId) : IDomainEvent;