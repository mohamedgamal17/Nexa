using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.Fakers
{
    public class FakeKYCServiceProvider : IKYCProvider
    {
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

            return Task.FromResult(kycCheck);
        }

        public Task<KYCClient> CreateClientAsync(KYCClientRequest request, CancellationToken cancellationToken = default)
        {
            var response = new KYCClient
            {
                Id = Guid.NewGuid().ToString(),
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                EmailAddress = request.EmailAddress,
                Nationality = request.Nationality,
                BirthDate = request.BirthDate,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender
            };

            return Task.FromResult(response);
        }
        public Task<KYCClient> UpdateClientAsync(string clientId, KYCClientRequest request, CancellationToken cancellationToken = default)
        {
            var response = new KYCClient
            {
                Id = clientId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                MiddleName = request.MiddleName,
                EmailAddress = request.EmailAddress,
                Nationality = request.Nationality,
                BirthDate = request.BirthDate,
                PhoneNumber = request.PhoneNumber,
                Gender = request.Gender
            };

            return Task.FromResult(response);

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

            return Task.FromResult(response);
        }

        public Task<KYCDocument> GetDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var response = new KYCDocument
            {
                Id = documentId,
                ClientId = Guid.NewGuid().ToString(),
                IssuingCountry = "US",
                Type = DocumentType.Passport
            };

            return Task.FromResult(response);
        }
        public Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
        

        public Task DeleteDocumentAttachementAsync(string documentId, string attachmentId, CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }
        public Task<KYCDocumentAttachement> UploadDocumentAttachementAsync(string documentId, KYCDocumentAttachmentRequest request, CancellationToken cancellationToken = default)
        {
            var response = new KYCDocumentAttachement
            {
                Id = Guid.NewGuid().ToString(),
                Size = 564545,
                FileName = request.FileName,
                ContentType = "imag/jpg",
                Side = request.Side,
                DownloadLink = Guid.NewGuid().ToString()
            };

            return Task.FromResult(response);
        }
        public Task<KYCDocumentAttachement> DownloadDocumentAttachementAsync(string documentId, string attachmentId)
        {
            var attachment = new KYCDocumentAttachement
            {
                Id = attachmentId,
                FileName = Guid.NewGuid().ToString(),
                Side = DocumentSide.Front,
                Size = 655445,
                ContentType = "imag/jpg",
                DownloadLink = Guid.NewGuid().ToString()
            };

            return Task.FromResult(attachment);
        }

        public Task<KYCCheck> GetCheckAsync(string checkId, CancellationToken cancellationToken = default)
        {
            var kycCheck = new KYCCheck
            {
                Id = checkId,
                ClientId = Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid().ToString(),
                Status = KYCCheckStatus.Pending,
                Type = KYCCheckType.DocumentCheck
            };

            return Task.FromResult(kycCheck);
        }




    
    }
}
