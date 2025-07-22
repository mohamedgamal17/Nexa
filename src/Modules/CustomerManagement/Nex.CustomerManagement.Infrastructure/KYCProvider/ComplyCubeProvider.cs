using ComplyCube.Net;
using ComplyCube.Net.Model;
using ComplyCube.Net.Resources.Addresses;
using ComplyCube.Net.Resources.Checks;
using ComplyCube.Net.Resources.Clients;
using ComplyCube.Net.Resources.Documents;
using ComplyCube.Net.Resources.Images;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Infrastructure.KYCProvider
{
    public class ComplyCubeProvider : IKYCProvider
    {
        private readonly ComplyCubeConfiguration _configuration;

        private readonly ComplyCubeClient _client;

        private readonly ClientApi _clientApi;

        private readonly AddressApi _addressApi;

        private readonly DocumentApi _documentApi;

        private readonly CheckApi _checkApi;
        public ComplyCubeProvider(ComplyCubeConfiguration configuration)
        {
            _configuration = configuration;
            _client = new ComplyCubeClient(_configuration.ApiKey);
            _clientApi = new ClientApi(_client);
            _addressApi = new AddressApi(_client);
            _documentApi = new DocumentApi(_client);
            _checkApi = new CheckApi(_client);
        }
        public async Task<KYCClient> CreateClientAsync(KYCClientRequest request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new ClientRequest
            {
                email = request.EmailAddress,
                type = "person",
                mobile = request.PhoneNumber,

            };

            var response = await _clientApi.CreateAsync(apiRequest);

            return PrepareKYCClient(response);
        }

        public async Task<KYCClient> UpdateClientInfoAsync(string clientId,KYCClientInfo request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new ClientRequest
            {
                personDetails = new PersonDetails
                {
                    firstName = request.FirstName,
                    lastName = request.LastName,
                    nationality = request.Nationality,
                    gender = MapComplyCupeGender(request.Gender),
                    dob = request.BirthDate.ToString()
                }
            };

            var response = await _clientApi.UpdateAsync(clientId, apiRequest);

            if(request.Address != null)
            {
                var addressRequest = new AddressRequest
                {
                    clientId = clientId,
                    country = request.Address.Country,
                    city = request.Address.City,
                    state = request.Address.State,
                    type = "main",
                    postalCode = request.Address.PostalCode,
                    line = request.Address.StreetLine
                };

                var clientAddresses = await _addressApi.ListAsync(clientId);

                if(clientAddresses.totalSize > 0)
                {
                    var mainId =  clientAddresses.items.Single(x => x.type == "main").id;

                    await _addressApi.UpdateAsync(mainId, addressRequest);
                }
                else
                {
                    await _addressApi.CreateAsync(addressRequest);
                }
            }

            return PrepareKYCClient(response, request.Address);
        }

        public async Task<KYCClient> UpdateClientAsync(string clientId, KYCClientRequest request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new ClientRequest
            {
                email = request.EmailAddress,
                type = "person",
                mobile = request.PhoneNumber,

            };

            var response = await _clientApi.UpdateAsync(clientId, apiRequest);

            return PrepareKYCClient(response);
        }

        public async Task<KYCDocument> CreateDocumentAsync(KYCDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new DocumentRequest
            {
                clientId = request.ClientId,
                type = request.Type == DocumentType.Passport ? "passport" : "driving_license",
                issuingCountry = request.IssuingCountry
            };

            var response =  await _documentApi.CreateAsync(apiRequest);

            return PrepareKYCDocument(response);
        }

        public async Task DeleteDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            await _documentApi.DeleteAsync(documentId);
        }
        public async Task<KYCDocument> GetDocumentAsync(string documentId, CancellationToken cancellationToken = default)
        {
            var response = await _documentApi.GetAsync(documentId);

            return PrepareKYCDocument(response);
        }
        public async Task<KYCDocumentAttachement> UploadDocumentAttachementAsync(string documentId, KYCDocumentAttachmentRequest request, CancellationToken cancellationToken = default)
        {
            var docSide = request.Side == DocumentSide.Front ? "front" : "back";

            var apiRequest = new ImageRequest
            {
                data = await ConvertStreamToBase64(request.Data),
                fileName = request.FileName
            };

            var response = await _documentApi.UploadImageAsync(documentId, docSide, apiRequest);

            return PrepareKYCDocumentAttachement(response);
        }

        public async Task<KYCDocumentAttachement> DownloadDocumentAttachementAsync(string documentId, string attachmentId)
        {
            var document = await _documentApi.GetAsync(documentId);

            var attachment = document.images.Single(x => x.id == attachmentId);

            return PrepareKYCDocumentAttachement(attachment);
        }

        public async Task DeleteDocumentAttachementAsync(string documentId, string attachmentId, CancellationToken cancellationToken = default)
        {
            var document = await _documentApi.GetAsync(documentId);

            var attachment = document.images.Single(x => x.id == attachmentId);

            await _documentApi.DeleteImageAsync(documentId, attachment.documentSide);

        }

        public async Task<KYCCheck> CreateCheckAsync(KYCCheckRequest request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new CheckRequest
            {
                clientId = request.ClientId,
                documentId = request.DocumentId,
                type = request.Type == KYCCheckType.DocumentCheck ? "document_check" : "identity_check"
            };

            var response = await _checkApi.CreateAsync(apiRequest);

            return PrepareKYCCheck(response);
        }

        public async Task<KYCCheck> GetCheckAsync(string checkId, CancellationToken cancellationToken = default)
        {
            var response = await _checkApi.GetAsync(checkId);

            return PrepareKYCCheck(response);
        }

     

        private string MapComplyCupeGender(Gender gender)
        {
            return gender switch
            {
                Gender.Male => "male",
                _ => "female"
            };
        }

        private KYCCheckStatus MapKYCStatus(string status)
        {
            return status switch
            {
                "pending" => KYCCheckStatus.Pending,
                "completed" => KYCCheckStatus.Completed,
                _ => KYCCheckStatus.Faild
            };
        }
        private KYCClient PrepareKYCClient(Client client, Domain.Customers.Address? address = null)
        {
            var response = new KYCClient
            {
                Id = client.id,
                EmailAddress = client.email,
                PhoneNumber = client.mobile
            };

            if(client.personDetails != null)
            {
                response.Info = new KYCClientInfo
                {
                    FirstName = client.personDetails.firstName,
                    LastName = client.personDetails.lastName,
                    BirthDate = DateTime.Parse(client.personDetails.dob),
                    Nationality = client.personDetails.nationality,
                    Gender = client.personDetails.gender == "male" ? Gender.Male : Gender.Female,

                };

                if(address != null)
                {
                    response.Info.Address = address;
                }             
            }

            return response;
        }

        private KYCDocument  PrepareKYCDocument(ComplyCube.Net.Model.Document document)
        {
            var response = new KYCDocument
            {
                Id = document.id,
                ClientId = document.clientId,
                IssuingCountry = document.issuingCountry,
                Type = document.type == "passport" ? DocumentType.Passport : DocumentType.DrivingLicense
            };

            return response;
        }

        private KYCDocumentAttachement PrepareKYCDocumentAttachement(Image image )
        {
           
            var response = new KYCDocumentAttachement
            {
                Id = image.id,
                FileName = image.fileName,
                Side = image.documentSide == "front" ? DocumentSide.Front : DocumentSide.Back,
                ContentType = image.contentType,
                Size = image.size,
                DownloadLink = image.downloadLink
            };

            return response;
        }

        private KYCCheck PrepareKYCCheck(Check check)
        {
            var response = new KYCCheck
            {
                Id = check.id,
                ClientId = check.clientId,
                DocumentId = check.documentId,
                Status = MapKYCStatus(check.status)
            };

            return response;
        }

        private async Task<string> ConvertStreamToBase64(Stream stream)
        {
            using (var memoryStream = new MemoryStream())
            {
                await stream.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();
                return Convert.ToBase64String(imageBytes);
            }
        }
    }
}
