namespace Nexa.BuildingBlocks.Domain.Exceptions
{
    public class BusinessLogicException : NexaException
    {
        public BusinessLogicException(string code , string message) 
            : base(code , message)
        {

        }
    }
}
