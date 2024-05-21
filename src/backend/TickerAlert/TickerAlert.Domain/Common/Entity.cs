namespace TickerAlert.Domain.Common
{
    public abstract class Entity
    {
        private readonly List<IDomainEvent> _domainEvents = new();
        public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
        public void ClearDomainEvents() => _domainEvents.Clear();
        public void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        
        public Guid Id { get; private set; }

        protected Entity(Guid id) => Id = id;
        
        // EF core required.
        private Entity() => Id = Guid.Empty;
        
    }
}
