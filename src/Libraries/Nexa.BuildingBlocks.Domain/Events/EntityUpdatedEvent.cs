namespace Nexa.BuildingBlocks.Domain.Events
{
    public class EntityUpdatedEvent<TEntity> : IEvent
    {
        public TEntity Entity { get; set; }

        public EntityUpdatedEvent(TEntity entity)
        {
            Entity = entity;
        }
    }

}
