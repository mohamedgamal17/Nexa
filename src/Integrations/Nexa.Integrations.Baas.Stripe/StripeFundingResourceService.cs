using Nexa.Integrations.Baas.Abstractions.Contracts.FundingResources;
using Nexa.Integrations.Baas.Abstractions.Services;
using Stripe;

namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeFundingResourceService : IBaasFundingResourceService
    {
        private readonly PaymentMethodService _paymentMethodService;
        public StripeFundingResourceService()
        {
            _paymentMethodService = new PaymentMethodService();
            
        }
        public async Task<BaasBankAccount> CreateBankAccountAsync(string accountId, BaasBankAccountCreateRequest request, CancellationToken cancellationToken = default)
        {

            var apiRequest = new PaymentMethodAttachOptions
            {
               Customer = accountId
            };

            var response = await _paymentMethodService.AttachAsync(request.Token, apiRequest);

            return PrepareBaasBankAccount(response);
        }

        public async Task<BaasBankAccount> GetBankAccountAsync(string accountId, string bankAccountId, CancellationToken cancellationToken = default)
        {
            var response = await _paymentMethodService.GetAsync(bankAccountId);

            return PrepareBaasBankAccount(response);
        }

        private BaasBankAccount PrepareBaasBankAccount(PaymentMethod paymentMethod)
        {
            var response = new BaasBankAccount
            {
                Id = paymentMethod.Id,
                HolderName = paymentMethod.UsBankAccount.BankName,               
                BankName = paymentMethod.UsBankAccount.BankName,
                RoutingNumber = paymentMethod.UsBankAccount.RoutingNumber,
                AccountNumberLast4 = paymentMethod.UsBankAccount.Last4,
                Country = "us",
                Currency = "usd"
            };

            return response;
        }
    }
}
