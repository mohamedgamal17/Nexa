using Nexa.Integrations.Baas.Abstractions.Contracts.Clients;
using Nexa.Integrations.Baas.Abstractions.Services;
using Stripe;

namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeClientService : IBaasClientService
    {
        private readonly AccountService _accountService;
        private readonly FileService _fileService;
        public StripeClientService()
        {
            _accountService = new AccountService();
            _fileService = new FileService();
        }
        public async Task<BaasClient> CreateClientAsync(CreateBaasClientRequest request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new AccountCreateOptions
            {
                Type = "custom",
                Country = "US",
                Email = request.Email,
                BusinessType = "individual",
                BusinessProfile = new AccountBusinessProfileOptions
                {
                    Name = "Nexa Wallet",
                    Mcc = "6012",
                    ProductDescription = "Digital wallet for peer-to-peer transfers",
                },

                Capabilities = new AccountCapabilitiesOptions
                {
                    CardPayments = new AccountCapabilitiesCardPaymentsOptions { Requested = true },
                    Transfers = new AccountCapabilitiesTransfersOptions { Requested = true },
                    Treasury = new AccountCapabilitiesTreasuryOptions { Requested = true }
                },

                Settings = new AccountSettingsOptions
                {
                    Treasury = new AccountSettingsTreasuryOptions
                    {
                        TosAcceptance = new AccountSettingsTreasuryTosAcceptanceOptions
                        {
                            Date = DateTime.UtcNow,
                            Ip = "203.0.113.1"
                        }
                    },

                },
                Individual = new AccountIndividualOptions
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    IdNumber = request.SSN,
                    Dob = new DobOptions { Day = request.DateOfBirth.Day, Month = request.DateOfBirth.Month, Year = request.DateOfBirth.Year },
                    Address = new AddressOptions
                    {
                        Line1 = request.Address.StreetLine,
                        City = request.Address.City,
                        State = request.Address.State,
                        PostalCode = request.Address.PostalCode,
                        Country = request.Address.Country
                    },
                    Email = request.Email,
                    Phone = request.PhoneNumber,
                },
                TosAcceptance = new AccountTosAcceptanceOptions
                {
                    Date = DateTime.UtcNow,
                    Ip = "203.0.113.1", 
                },
            };

            var response = await _accountService.CreateAsync(apiRequest);

            return PrepareBaasClient(response);
        }

        public async Task<BaasClient> GetClientAsync(string clientId, CancellationToken cancellationToken = default)
        {
            var response = await _accountService.GetAsync(clientId);

            return PrepareBaasClient(response);
        }

        public async Task<BaasClient> UploadDocument(string clientId, UploadDocumentRequest request, CancellationToken cancellationToken = default)
        {
           

            var apiRequest = new AccountUpdateOptions
            {
                Individual = new AccountIndividualOptions
                {
                    Verification = new AccountIndividualVerificationOptions
                    {
                        Document = new AccountIndividualVerificationDocumentOptions
                        {
                            Front = (await _fileService.CreateAsync(new FileCreateOptions() { File = request.Front })).Id,
                            Back = request.Back != null ?
                                (await _fileService.CreateAsync(new FileCreateOptions() { File = request.Back })).Id
                                : null
                        }
                    }
                }
            };


            var response = await _accountService.UpdateAsync(clientId, apiRequest);

            return PrepareBaasClient(response);
        }

        private BaasClient PrepareBaasClient(Account account)
        {

            var client = new BaasClient
            {
                Id = account.Id,
                Email = account.Email,
                FirstName = account.Individual!.FirstName,
                LastName = account.Individual!.LastName,
                DateOfBirth = new DateTime((int)account.Individual.Dob.Year!, (int)account.Individual.Dob.Month!, (int)account.Individual.Dob.Day!),
                Gender = account.Individual.Gender == "male" ? Gender.Male : Gender.Female,
                PhoneNumber = account.Individual.Phone,
                Address = new Abstractions.Contracts.Clients.Address
                {
                    Country = account.Individual.Address.Country,
                    City = account.Individual.Address.City,
                    State = account.Individual.Address.State,
                    StreetLine = account.Individual.Address.Line1,
                    PostalCode = account.Individual.Address.PostalCode
                }
            };

            return client;
        }
    }
}
