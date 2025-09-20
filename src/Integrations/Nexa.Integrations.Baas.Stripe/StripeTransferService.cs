using MediatR;
using Nexa.Integrations.Baas.Abstractions.Contracts.Transfers;
using Nexa.Integrations.Baas.Abstractions.Services;
using Stripe;
using Stripe.Treasury;

namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeTransferService : IBaasTransferService
    {
        private readonly InboundTransferService _inboundTransferService;
        private readonly SetupIntentService _setupIntentService;
        public StripeTransferService()
        {
            _inboundTransferService = new InboundTransferService();
            _setupIntentService = new SetupIntentService();
        }
        public async Task<BaasDepositTransfer> Deposit(DepositTransferRequest request, CancellationToken cancellationToken = default)
        {
            var requestOptions = new RequestOptions { StripeAccount = request.AccountId };

            var setupIntentRequest = new SetupIntentCreateOptions
            {
                PaymentMethod = request.FundingResourceId,
                Confirm = true,
                MandateData = new SetupIntentMandateDataOptions
                {
                    CustomerAcceptance = new SetupIntentMandateDataCustomerAcceptanceOptions
                    {
                        Type = "offline",
                    },
                },
                AutomaticPaymentMethods = new SetupIntentAutomaticPaymentMethodsOptions { Enabled = true, AllowRedirects = "never" }
            };

            await _setupIntentService.CreateAsync(setupIntentRequest, requestOptions);

            var inboundTransferRequest = new InboundTransferCreateOptions
            {
                FinancialAccount = request.WalletId,
                Currency = "usd",
                OriginPaymentMethod = request.FundingResourceId,
                Amount = (long)request.Amount * 100,
                Metadata = new Dictionary<string, string>
                {
                    { "ClinetTransferId" , request.ClinetTransferId }
                }
            };

            var response = await _inboundTransferService.CreateAsync(inboundTransferRequest, requestOptions);

            var result = new BaasDepositTransfer
            {
                Id = response.Id,
                WalletId = response.FinancialAccount,
                Amount = response.Amount / 100,
                FundingResource = response.OriginPaymentMethod
            };


            return result;
        }
    }
}
