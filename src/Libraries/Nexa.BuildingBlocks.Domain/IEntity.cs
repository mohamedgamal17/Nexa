namespace Nexa.BuildingBlocks.Domain
{
    public interface  IEntity
    {

    }
    public interface IEntity<TId> : IEntity
    {
        TId Id { get;  }
    }
 
}
