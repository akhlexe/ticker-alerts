namespace TickerAlert.Domain.Common
{
    public abstract class Entity
    {
        public int Id { get; private set; }
        
        protected Entity(int id = 0) => Id = id;
    }
}
