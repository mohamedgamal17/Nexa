using Nexa.Integrations.Baas.Abstractions.Configuration;
using Nexa.Integrations.Baas.Abstractions.Contracts.FundingResources;
using Nexa.Integrations.Baas.Abstractions.Services;
using Stripe;

namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeFundingResourceService : IBaasFundingResourceService
    {
        private readonly AccountExternalAccountService accountExternalAccountService;
        public StripeFundingResourceService(BaasConfiguration baasConfiguration)
        {
            accountExternalAccountService = new AccountExternalAccountService();
            
        }
        public async Task<BaasBankAccount> CreateBankAccountAsync(string accountId, BaasBankAccountCreateRequest request, CancellationToken cancellationToken = default)
        {

            var apiRequest = new AccountExternalAccountCreateOptions
            {
                ExternalAccount = request.Token
            };


            var response = await accountExternalAccountService.CreateAsync(accountId, apiRequest);


            return PrepareBaasBankAccount((BankAccount)response);

        }

        public async Task<BaasBankAccount> GetBankAccountAsync(string accountId, string bankAccountId, CancellationToken cancellationToken = default)
        {
            var response = await accountExternalAccountService.GetAsync(accountId,bankAccountId );

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
                Country = "us",
                Currency = "usd"
            };

            return response;
        }
    }
}
