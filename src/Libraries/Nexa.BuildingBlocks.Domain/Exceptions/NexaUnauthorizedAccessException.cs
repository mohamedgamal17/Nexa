namespace Nexa.BuildingBlocks.Domain.Exceptions
{
    public class NexaUnauthorizedAccessException : NexaException
    {
        public NexaUnauthorizedAccessException()
        {

        }

        public NexaUnauthorizedAccessException(string code, string? message = null) : base(message)
        {
        }

        public NexaUnauthorizedAccessException(NexaError error, List<object>? data = null) : base(error) { }
    }
}
