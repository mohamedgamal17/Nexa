using MediatR;
using Nexa.BuildingBlocks.Domain.Events;
namespace Nexa.BuildingBlocks.Domain
{
    public interface IAggregateRoot : IEntity
    {
        IReadOnlyList<IEvent> Events { get; }
        void ClearDomainEvents();
    }
    public interface IAggregateRoot<TKey> : IAggregateRoot, IEntity<TKey>
    {

    }
  
}
