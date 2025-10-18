namespace Nexa.BuildingBlocks.Domain.Exceptions
{
    public class BusinessLogicException : NexaException
    {
        public BusinessLogicException(string code , string? message = null) 
            : base(code , message)
        {

        }
        public BusinessLogicException(NexaError error ,  List<object>? data = null) : base(error, data) { } 
    }
}
