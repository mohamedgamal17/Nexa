namespace Nexa.BuildingBlocks.Domain.Exceptions
{
    public class NexaException : Exception
    {
        public string Code { get; set; }
        public List<object> Params { get; set; }
        public NexaException(string code) : base(code)
        {
            Code = code;
            Params = new List<object>();
        }
        public NexaException WithData(object data)
        {
            Params.Add(data);
            return this;
        }
    }
}
