namespace Nexa.BuildingBlocks.Domain.Exceptions
{
    public class NexaException : Exception
    {
        public string Code { get; set; }
        public List<object> Params { get; set; }

        public NexaException()
        {
            
        }
        public NexaException(string code  , string? message = null , List<object>? data = null) : base(message ?? code)
        {
            Code = code;
            Params = data ?? new List<object>();
        }

        public NexaException(NexaError error , List<object>? data = null)
            :base(string.Format(error.Message,data ?? new List<object>()))
        {
            Code = error.Code;
            Params = data ?? new List<object>();
        }
        public NexaException WithData(object data)
        {
            Params.Add(data);
            return this;
        }
    }
}
