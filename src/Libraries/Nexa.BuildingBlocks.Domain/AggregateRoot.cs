using Nexa.BuildingBlocks.Domain.Events;

namespace Nexa.BuildingBlocks.Domain
{
    public class AggregateRoot<TKey> : BaseEntity<TKey>, IAggregateRoot<TKey>
    {
        private readonly List<IEvent> _events = new List<IEvent>();

        public AggregateRoot(TKey id) : base(id)
        {

        }

        public IReadOnlyList<IEvent> Events => _events.AsReadOnly();
        protected void AppendEvent(IEvent @event) => _events.Add(@event);
        public void ClearDomainEvents()
        {
            _events.Clear();
        }

    }

    public class AggregateRoot : AggregateRoot<string>
    {
        public AggregateRoot(string id) : base(id)
        {

        }

        public AggregateRoot() 
            : base(Guid.NewGuid().ToString())
        {
            
        }
    }
}
