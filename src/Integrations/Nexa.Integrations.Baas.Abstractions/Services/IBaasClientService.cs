using Nexa.Integrations.Baas.Abstractions.Contracts.Clients;

namespace Nexa.Integrations.Baas.Abstractions.Services
{
    public interface IBaasClientService
    {
        Task<BaasClient> CreateClientAsync(CreateBaasClientRequest request, CancellationToken cancellationToken = default);
        Task<BaasClient> GetClientAsync(string clientId, CancellationToken cancellationToken = default);
        Task<BaasClient> UploadDocument(string clientId, UploadDocumentRequest request, CancellationToken cancellationToken = default);
    }
}
