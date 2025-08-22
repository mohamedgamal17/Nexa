using Nexa.Integrations.Baas.Abstractions.Contracts.Wallets;
using Nexa.Integrations.Baas.Abstractions.Services;
using Stripe;
using Stripe.Treasury;
namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeWalletService : IBaasWalletService
    {
        private readonly FinancialAccountService _financialAccountService;
        public StripeWalletService()
        {
            _financialAccountService = new FinancialAccountService();
        }
        public async Task<BaasWallet> CreateWalletAsync(string clientId, CancellationToken cancellationToken = default)
        {


            var request = new FinancialAccountCreateOptions
            {
                
                SupportedCurrencies = new List<string>
                {
                    "usd"
                },
                Features = new FinancialAccountFeaturesOptions
                {

                    InboundTransfers = new FinancialAccountFeaturesInboundTransfersOptions
                    {
                        Ach = new FinancialAccountFeaturesInboundTransfersAchOptions
                        {
                            Requested = true
                        }
                    },
                    OutboundTransfers = new FinancialAccountFeaturesOutboundTransfersOptions
                    {
                        Ach = new FinancialAccountFeaturesOutboundTransfersAchOptions
                        {
                            Requested = true
                        }

                    },

                    IntraStripeFlows = new FinancialAccountFeaturesIntraStripeFlowsOptions
                    {
                        Requested = true
                    }
                },
            };

            var options = new RequestOptions
            {
                StripeAccount = clientId
            };

            var response = await _financialAccountService.CreateAsync(request, options);

            var baasWallet = new BaasWallet
            {
                Id = response.Id,
                Balance = response.Balance.Cash["usd"]
            };

            return baasWallet;
        }
    }
}
