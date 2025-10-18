namespace Nexa.BuildingBlocks.Domain.Exceptions
{
    public class ForbiddenAccessException : NexaException
    {
        public ForbiddenAccessException()
        {
            
        }
     

        public ForbiddenAccessException(string code  , string? message = null) : base(message)
        {
        }

        public ForbiddenAccessException(NexaError error,  List<object>? data = null) : base(error) { }
    }
}
