namespace Nexa.BuildingBlocks.Domain
{
    public abstract class EntityView<TId>
    {
        public TId Id { get; set; }
    }
    public abstract class EntityView : EntityView<string>
    {

    }

   
}
