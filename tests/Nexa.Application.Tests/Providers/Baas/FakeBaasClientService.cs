using Nexa.Integrations.Baas.Abstractions.Contracts.Clients;
using Nexa.Integrations.Baas.Abstractions.Services;

namespace Nexa.Application.Tests.Providers.Baas
{
    public class FakeBaasClientService : IBaasClientService
    {
        private readonly static List<BaasClient> _clients = new List<BaasClient>();

        public Task<BaasClient> CreateClientAsync(CreateBaasClientRequest request, CancellationToken cancellationToken = default)
        {
            var client = new BaasClient
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                SSN = request.SSN,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender,
                Address = request.Address,
                DateOfBirth = request.DateOfBirth,
                Email = request.Email
            };

            _clients.Add(client);

            return Task.FromResult(client);
        }

        public Task<BaasClient> GetClientAsync(string clientId, CancellationToken cancellationToken = default)
        {
            var client = _clients.Single(x => x.Id == clientId);

            return Task.FromResult(client);
        }

        public Task<BaasClient> UploadDocument(string clientId, UploadDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var client = _clients.Single(x => x.Id == clientId);

            return Task.FromResult(client);
        }
    }
}
