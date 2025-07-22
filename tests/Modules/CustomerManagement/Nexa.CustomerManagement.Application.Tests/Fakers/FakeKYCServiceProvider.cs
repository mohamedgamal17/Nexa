using Nexa.CustomerManagement.Domain.KYC;
namespace Nexa.CustomerManagement.Application.Tests.Fakers
{
    public class FakeKYCServiceProvider : IKYCProvider
    {
        private readonly static List<KYCClient> _kycClients = new List<KYCClient>();

        private readonly static List<KYCDocument> _kycDocuments = new List<KYCDocument>();

        private readonly static List<KYCCheck> _kycCheck = new List<KYCCheck>();
        public Task<KYCCheck> CreateCheckAsync(KYCCheckRequest request, CancellationToken cancellationToken = default)
        {
            var kycCheck = new KYCCheck
            {
                Id = Guid.NewGuid().ToString(),
                Type = request.Type,
                DocumentId = request.DocumentId,
                ClientId = request.ClientId,
                Status = KYCCheckStatus.Pending
            };

            _kycCheck.Add(kycCheck);

            return Task.FromResult(kycCheck);
        }

        public Task<KYCClient> CreateClientAsync(KYCClientRequest request, CancellationToken cancellationToken = default)
        {
            var response = new KYCClient
            {
                Id = Guid.NewGuid().ToString(),
                EmailAddress = request.EmailAddress,
                PhoneNumber = request.PhoneNumber,

            };

            _kycClients.Add(response);

            return Task.FromResult(response);
        }
        public Task<KYCClient> UpdateClientAsync(string clientId, KYCClientRequest request, CancellationToken cancellationToken = default)
        {
            var client = _kycClients.Single(x => x.Id == clientId);

            client.EmailAddress = request.EmailAddress;

            client.PhoneNumber = request.PhoneNumber;

            return Task.FromResult(client);

        }

        public Task<KYCClient> UpdateClientInfoAsync(string clientId, KYCClientInfo request, CancellationToken cancellationToken = default)
        {
            var client = _kycClients.Single(x => x.Id == clientId);

            client.Info = request;

            return Task.FromResult(client);
        }
        public Task<KYCDocument> CreateDocumentAsync(KYCDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var response = new KYCDocument
            {
                Id = Guid.NewGuid().ToString(),
                ClientId = request.ClientId,
                IssuingCountry = request.IssuingCountry,
                Type = request.Type
            };

            _kycDocuments.Add(response);

            return Task.FromResult(response);
        }

        public Task<KYCDocument> GetDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var response = _kycDocuments.Single(x => x.Id == documentId);

            return Task.FromResult(response);
        }
        public Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var data =  _kycDocuments.Single(x => x.Id == documentId);

            _kycDocuments.Remove(data);

            return Task.CompletedTask;
        }
        

        public Task DeleteDocumentAttachementAsync(string documentId, string attachmentId, CancellationToken cancellationToken = default)
        {
            var data = _kycDocuments.Single(x => x.Id == documentId);

            var attachment =  data.Attachements?.SingleOrDefault(x => x.Id == attachmentId);

            if(attachment != null)
            {
                data.Attachements!.Remove(attachment);
            }
           
            return Task.CompletedTask;
        }
        public Task<KYCDocumentAttachement> UploadDocumentAttachementAsync(string documentId, KYCDocumentAttachmentRequest request, CancellationToken cancellationToken = default)
        {
            var document = _kycDocuments.Single(x => x.Id == documentId);

            var response = new KYCDocumentAttachement
            {
                Id = Guid.NewGuid().ToString(),
                Size = 564545,
                FileName = request.FileName,
                ContentType = "imag/jpg",
                Side = request.Side,
                DownloadLink = Guid.NewGuid().ToString()
            };

            if (document.Attachements == null)
                document.Attachements = new List<KYCDocumentAttachement>();

            document.Attachements.Add(response);

            return Task.FromResult(response);
        }
        public Task<KYCDocumentAttachement> DownloadDocumentAttachementAsync(string documentId, string attachmentId)
        {
            var document = _kycDocuments.Single(x => x.Id == documentId);

            var attachment = document.Attachements!.Single(x => x.Id == attachmentId);

            return Task.FromResult(attachment);
        }

        public Task<KYCCheck> GetCheckAsync(string checkId, CancellationToken cancellationToken = default)
        {
            var kycCheck = _kycCheck.Single(x => x.Id == checkId);

            return Task.FromResult(kycCheck);
        }

      
    }
}
