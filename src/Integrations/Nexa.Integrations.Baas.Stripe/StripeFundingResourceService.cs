using Nexa.Integrations.Baas.Abstractions.Contracts.FundingResources;
using Nexa.Integrations.Baas.Abstractions.Services;
using Stripe;

namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeFundingResourceService : IBaasFundingResourceService
    {
        private readonly PaymentMethodService _paymentMethodService;
        private readonly CustomerService _customerService;
        public StripeFundingResourceService()
        {
            _paymentMethodService = new PaymentMethodService();
            _customerService = new CustomerService();
            
        }
        public async Task<BaasBankAccount> CreateBankAccountAsync(string accountId, BaasBankAccountCreateRequest request, CancellationToken cancellationToken = default)
        {
            if (request.Token.StartsWith("pm"))
            {
                var response = await _paymentMethodService.GetAsync(request.Token);

                return PrepareBaasBankAccount(response);
            }
            else
            {

                var customer = await _customerService.GetAsync(accountId);

                var apiRequest = new PaymentMethodCreateOptions
                {
                    Customer = accountId,

                    UsBankAccount = new PaymentMethodUsBankAccountOptions
                    {
                        FinancialConnectionsAccount = request.Token,
                        
                    },
                    BillingDetails = new PaymentMethodBillingDetailsOptions
                    {
                        Name = customer.Name,
                        Email = customer.Email
                    }
                };

                var response = await _paymentMethodService.CreateAsync(apiRequest);

                return PrepareBaasBankAccount(response);

            }
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
                HolderName = paymentMethod.BillingDetails.Name,               
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
