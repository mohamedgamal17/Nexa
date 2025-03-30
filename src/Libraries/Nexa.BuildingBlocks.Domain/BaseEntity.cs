namespace Nexa.BuildingBlocks.Domain
{
    public class BaseEntity<TId> : IEntity<TId>
    {
        public TId Id { get; protected set; }


        public BaseEntity(TId id)
        {
            Id = id;
        }
    }

    public class BaseEntity : BaseEntity<string>
    {
        public BaseEntity()
            : base(Guid.NewGuid().ToString())
        {

        }
        public BaseEntity(string id) : base(id) { }
    }
}
