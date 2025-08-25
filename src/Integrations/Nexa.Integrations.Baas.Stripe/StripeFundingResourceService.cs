using Nexa.Integrations.Baas.Abstractions.Configuration;
using Nexa.Integrations.Baas.Abstractions.Contracts.FundingResources;
using Nexa.Integrations.Baas.Abstractions.Services;
using Stripe;

namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeFundingResourceService : IBaasFundingResourceService
    {
        private readonly AccountExternalAccountService _bankAccountService;
        private readonly BaasConfiguration _baasConfiguration;
        public StripeFundingResourceService(BaasConfiguration baasConfiguration)
        {
            _bankAccountService = new AccountExternalAccountService();
            _baasConfiguration = baasConfiguration;
        }
        public async Task<BaasBankAccount> CreateBankAccountAsync(string accountId, BaasBankAccountCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (!_baasConfiguration.IsDevlopment)
            {
                var apiRequest = new AccountExternalAccountCreateOptions
                {
                    ExternalAccount = request.Token
                };

                var response = await _bankAccountService.CreateAsync(accountId, apiRequest);

                return PrepareBaasBankAccount((BankAccount)response);

            }
            else
            {
                //this condition work only in stripe sandbox test mode not production
                //create verified bank account for connected account
                var apiRequest = new AccountExternalAccountCreateOptions
                {
                    ExternalAccount = new AccountExternalAccountBankAccountOptions
                    {
                        Country = "US",
                        Currency = "usd",
                        RoutingNumber = "110000000",
                        AccountNumber = "000123456789"
                    }
                };

                var response = await _bankAccountService.CreateAsync(accountId, apiRequest);

                return PrepareBaasBankAccount((BankAccount)response);
            }
        }

        public async Task<BaasBankAccount> GetBankAccountAsync(string accountId, string bankAccountId, CancellationToken cancellationToken = default)
        {
            var response = await _bankAccountService.GetAsync(accountId, bankAccountId);

            return PrepareBaasBankAccount((BankAccount)response);
        }

        private BaasBankAccount PrepareBaasBankAccount(BankAccount bankAccount)
        {
            var response = new BaasBankAccount
            {
                Id = bankAccount.Id,
                HolderName = bankAccount.AccountHolderName,
                BankName = bankAccount.BankName,
                RoutingNumber = bankAccount.RoutingNumber,
                AccountNumberLast4 = bankAccount.Last4,
                Country = bankAccount.Country,
                Currency = bankAccount.Currency
            };

            return response;
        }
    }
}
