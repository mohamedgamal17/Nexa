namespace Nexa.BuildingBlocks.Domain.Exceptions
{
    public class EntityNotFoundException : NexaException
    {
        public EntityNotFoundException(string code) : base(code)
        {
        }

        public EntityNotFoundException(Type entity, string id) : base(
               $"there is no such {entity.Name} entity with id : {id}"
           )
        {

        }

    }
}
