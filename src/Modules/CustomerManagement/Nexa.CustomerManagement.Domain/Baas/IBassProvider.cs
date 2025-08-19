namespace Nexa.CustomerManagement.Domain.Baas
{
    public interface IBassProvider
    {
        Task<BaasClient> CreateClientAsync(CreateBaasClient request, CancellationToken cancellationToken = default);
        Task<BaasClient> GetClientAsync(string clientId, CancellationToken cancellationToken = default);
        Task<BaasClient> UploadDocument(string clientId, UploadBaasDocument request, CancellationToken cancellationToken = default);
    }
}
