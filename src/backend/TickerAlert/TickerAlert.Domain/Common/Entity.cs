namespace TickerAlert.Domain.Common
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }

        protected Entity(Guid id) => Id = id;
        
        // EF core required.
        private Entity() => Id = Guid.Empty;
        
    }
}
