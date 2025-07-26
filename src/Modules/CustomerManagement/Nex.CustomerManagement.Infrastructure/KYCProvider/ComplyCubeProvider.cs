using ComplyCube.Net;
using ComplyCube.Net.Exceptions;
using ComplyCube.Net.Model;
using ComplyCube.Net.Resources.Addresses;
using ComplyCube.Net.Resources.Checks;
using ComplyCube.Net.Resources.Clients;
using ComplyCube.Net.Resources.Documents;
using ComplyCube.Net.Resources.Images;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Enums;
using System.Net.Http.Headers;
using System.Text.Json;
using ComplyCube.Net.Utils;
using System.Text.Json.Serialization;
using System.Text;

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
            _client = new ComplyCubeClient(_configuration.ApiKey, new HttpClient());
            _clientApi = new ClientApi(_client);
            _addressApi = new AddressApi(_client);
            _documentApi = new DocumentApi(_client);
            _checkApi = new CheckApi(_client);
        }
        public async Task<KYCClient> CreateClientAsync(KYCClientRequest request, CancellationToken cancellationToken = default)
        {

            var apiRequest = new ExtendedClientRequest
            {
                email = request.EmailAddress,
                mobile = request.PhoneNumber,
                type="person",
                personDetails = new ExtendedPersonDetails
                {
                    firstName = request.Info.FirstName,
                    lastName = request.Info.LastName,
                    dob = request.Info.BirthDate.ToString("yyyy-MM-dd"),
                    gender = MapComplyCupeGender(request.Info.Gender),
                    nationality = request.Info.Nationality,
                    ssn = request.Info.SSN
                }
            };


            var response = await SendCustomPostRequest<Client>("/clients", apiRequest);

                
            if (request.Info.Address != null)
            {
                var addressRequest = new AddressRequest
                {
                    clientId = response.id,
                    country = request.Info.Address.Country,
                    city = request.Info.Address.City,
                    state = request.Info.Address.State,
                    type = "main",
                    postalCode = request.Info.Address.PostalCode,
                    line = request.Info.Address.StreetLine
                };

                await _addressApi.CreateAsync(addressRequest);
            }

       
            return PrepareKYCClient(response, request.Info.Address);
        }



        public async Task<KYCClient> UpdateClientAsync(string clientId, KYCClientRequest request, CancellationToken cancellationToken = default)
        {

            var apiRequest = new ExtendedClientRequest
            {
                email = request.EmailAddress,
                mobile = request.PhoneNumber,
                personDetails = new ExtendedPersonDetails
                {
                    firstName = request.Info.FirstName,
                    lastName = request.Info.LastName,
                    dob = request.Info.BirthDate.ToString("yyyy-MM-dd"),
                    gender = MapComplyCupeGender(request.Info.Gender),
                    nationality = request.Info.Nationality,
                    ssn = request.Info.SSN
                },

            };


            var response = await SendCustomPostRequest<Client>($"clients/{clientId}", apiRequest);



            if (request.Info.Address != null)
            {
                var addressRequest = new AddressRequest
                {
                    clientId = clientId,
                    country = request.Info.Address.Country,
                    city = request.Info.Address.City,
                    state = request.Info.Address.State,
                    postalCode = request.Info.Address.PostalCode,
                    line = request.Info.Address.StreetLine
                };

                var clientAddresses = await _addressApi.ListAsync(clientId);

                if (clientAddresses.items != null && clientAddresses.items.Count() > 0)
                {
                    var mainId = clientAddresses.items.Single(x => x.type == "main").id;

                    await _addressApi.UpdateAsync(mainId, addressRequest);
                }
                else
                {
                    addressRequest.type = "main";

                    await _addressApi.CreateAsync(addressRequest);
                }
            }

            return PrepareKYCClient(response, request.Info.Address);

        }

        public async Task<KYCDocument> CreateDocumentAsync(KYCDocumentRequest request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new DocumentRequest
            {
                clientId = request.ClientId,
                type = request.Type == DocumentType.Passport ? "passport" : "driving_license",
                issuingCountry = request.IssuingCountry
            };

            var response = await _documentApi.CreateAsync(apiRequest);

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
            var apiRequest = new ExtendedCheckRequest
            {
                clientId = request.ClientId,
                documentId = request.DocumentId,
                type = MapCheckType(request.Type)
            };

            if (request.Type == KYCCheckType.IdNumberCheck)
            {
                var addresses = await _addressApi.ListAsync(request.ClientId);

                var mainAddress = addresses.items.Single(x => x.type == "main");

                apiRequest.addressId = mainAddress.id;
            }

            var response = await SendCustomPostRequest<Check>("/checks", apiRequest);

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

        private string MapCheckType(KYCCheckType checkType)
        {
            return checkType switch
            {
                KYCCheckType.IdNumberCheck => "multi_bureau_check",
                KYCCheckType.IdentityCheck => "identity_check",
                _ => "document_check"
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

            if (client.personDetails != null)
            {
                response.Info = new KYCClientInfo
                {
                    FirstName = client.personDetails.firstName,
                    LastName = client.personDetails.lastName,
                    BirthDate = DateTime.Parse(client.personDetails.dob),
                    Nationality = client.personDetails.nationality,
                    Gender = client.personDetails.gender == "male" ? Gender.Male : Gender.Female,

                };

                if (address != null)
                {
                    response.Info.Address = address;
                }
            }

            return response;
        }

        private KYCDocument PrepareKYCDocument(ComplyCube.Net.Model.Document document)
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

        private KYCDocumentAttachement PrepareKYCDocumentAttachement(Image image)
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
                LiveVideoId = check.livePhotoId,
              
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

        private async Task<T> SendCustomPostRequest<T>(string path, object body)
        {
            return await SendCustomerRequestAsync<T>(path, HttpMethod.Post, body);
        }


        private async Task<T> SendCustomerRequestAsync<T>(string path , HttpMethod httpMethod, object? data)
        {
            var client = CreateHttpClient();

            var httpRequestMessage = new HttpRequestMessage(httpMethod, $"{_client.baseUrl}/{path}");

            string version =_client.GetType().Assembly.GetName().Version?.ToString() ?? "1";

            ProductInfoHeaderValue item = new ProductInfoHeaderValue("complycube-dotnet", version);

            httpRequestMessage.Headers.UserAgent.Add(item);

            httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            httpRequestMessage.Headers.TryAddWithoutValidation("Authorization", _configuration.ApiKey);

            JsonSerializerOptions options = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            options.Converters.Add(new CustomDateTimeConverter());

            if (httpMethod.Equals(HttpMethod.Post))
            {
                string content = JsonSerializer.Serialize(data, options);
                StringContent stringContent = new StringContent(content, Encoding.UTF8, "application/json");
                httpRequestMessage.Content = stringContent;
                stringContent.Headers.ContentType.CharSet = "";
            }
            
            var httpResponseMessage = await client.SendAsync(httpRequestMessage);
            

            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new ComplyCubeServerException(httpResponseMessage.ReasonPhrase, httpResponseMessage);
            }
            
            return await SerialzeHttpResponseMessage<T>(httpResponseMessage);
        }

        private async Task<T> SerialzeHttpResponseMessage<T>(HttpResponseMessage httpResponseMessage)
        {
            var json = await httpResponseMessage.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<T>(json);

            return result;
        }
        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", _configuration.ApiKey);

            return httpClient;
        }

        public Task<bool> VerifiyWebHookSignature(string signature, string body)
        {
            var eventVerifier = new EventVerifier(_configuration.WebhookSecret);

            try
            {
                var webhookEvent = eventVerifier.ConstructEvent(body, signature);

                return Task.FromResult(webhookEvent != null);
            }
            catch (ComplyCube.Net.Exceptions.VerificationException) {
                return Task.FromResult(false);
            }
        }

        public Task<KYCDocument> UpdateDocumentAsync(string documentId, KYCDocumentRequest request, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
    public class ExtendedCheckRequest : CheckRequest
    {
        public string addressId { get; set; }
    }


    public class ExtendedClientRequest
    {
        public string type { get; set; }

        public string entityName { get; set; }

        public string email { get; set; }

        public string mobile { get; set; }

        public string telephone { get; set; }

        public string externalId { get; set; }

        public string joinedDate { get; set; }

        public ExtendedPersonDetails personDetails { get; set; }

        public CompanyDetails companyDetails { get; set; }
    }
    public class ExtendedPersonDetails : PersonDetails
    {
        public string ssn { get; set; }
    }
}
