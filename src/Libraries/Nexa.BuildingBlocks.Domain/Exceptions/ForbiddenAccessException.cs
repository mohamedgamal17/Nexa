namespace Nexa.BuildingBlocks.Domain.Exceptions
{
    public class ForbiddenAccessException : Exception
    {
        public ForbiddenAccessException()
        {
            
        }
        public ForbiddenAccessException(string message) : base(message)
        {
            
        }
    }
}
