namespace Nexa.BuildingBlocks.Domain.Exceptions
{
    public class ConflictException : NexaException
    {
        public ConflictException(string code, string? message = null)
           : base(code, message)
        {

        }
        public ConflictException(NexaError error, List<object>? data = null) : base(error, data) { }
    }
}
