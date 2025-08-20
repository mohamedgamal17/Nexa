namespace Nexa.Integrations.Baas.Abstractions.Contracts.Clients
{
    public class UploadDocumentRequest
    {
        public Stream Front { get; set; }
        public Stream? Back { get; set; }
    }
}
