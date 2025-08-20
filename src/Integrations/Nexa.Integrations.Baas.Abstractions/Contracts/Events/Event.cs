namespace Nexa.Integrations.Baas.Abstractions.Contracts.Events
{
    public class Event
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public object Data { get; set; }

    }
}
