using MediatR;
using Nexa.Integrations.Baas.Abstractions.Configuration;
using Nexa.Integrations.Baas.Abstractions.Contracts.Transfers;
using Nexa.Integrations.Baas.Abstractions.Services;
using Stripe;
using Stripe.Treasury;

namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeTransferService : IBaasTransferService
    {
        private readonly BaasConfiguration _baasConfiguration;
        private readonly InboundTransferService _inboundTransferService;
        private readonly SetupIntentService _setupIntentService;
        private readonly OutboundPaymentService _outboundPaymentService;
        private readonly OutboundTransferService _outboundTransferService;
        public StripeTransferService(BaasConfiguration baasConfiguration)
        {
            _inboundTransferService = new InboundTransferService();
            _setupIntentService = new SetupIntentService();
            _outboundPaymentService = new OutboundPaymentService();
            _outboundTransferService = new OutboundTransferService();
            _baasConfiguration = baasConfiguration;
        }
        public async Task<BaasBankTransfer> Deposit(BankTransferRequest request, CancellationToken cancellationToken = default)
        {
            var setupIntentRequest = new SetupIntentCreateOptions
            {
                AttachToSelf = true,
                PaymentMethod = request.FundingResourceId,
                PaymentMethodTypes  = ["us_bank_account"],
                Confirm = true,
                MandateData = new SetupIntentMandateDataOptions
                {
                    CustomerAcceptance = new SetupIntentMandateDataCustomerAcceptanceOptions
                    {
                        Type = "offline",
                    },
                },
            };

            await _setupIntentService.CreateAsync(setupIntentRequest);

            var inboundTransferRequest = new InboundTransferCreateOptions
            {
                Amount = (long)(request.Amount * 100),
                Currency = "usd",
                OriginPaymentMethod = request.FundingResourceId,
                FinancialAccount = _baasConfiguration.FinancialAccounts.Main,                          
                Metadata = new Dictionary<string, string>
                {
                    { StripeMetaDataConsts.ClientTransferId , request.ClientTransferId },
                }
            };

            var response = await _inboundTransferService.CreateAsync(inboundTransferRequest);

            var result = new BaasBankTransfer
            {
                Id = response.Id,
                WalletId = response.FinancialAccount,
                Amount = response.Amount / 100,
                FundingResourceId = response.OriginPaymentMethod
            };
            return result;
        }


        public async Task<BaasBankTransfer> Withdraw(BankTransferRequest request, CancellationToken cancellationToken = default)
        {
            var options = new OutboundTransferCreateOptions
            {
                Amount = (long)(request.Amount * 100),
                Currency = "usd",
                FinancialAccount = _baasConfiguration.FinancialAccounts.Main,
                DestinationPaymentMethod = request.FundingResourceId,
                Metadata = new Dictionary<string, string>
                {
                    {StripeMetaDataConsts.ClientTransferId , request.ClientTransferId }
                }
            };

            var response = await _outboundTransferService.CreateAsync(options);

            var result = new BaasBankTransfer
            {
                Id = response.Id,
                WalletId = response.FinancialAccount,
                Amount = response.Amount / 100,
                FundingResourceId = response.DestinationPaymentMethod
            };

            return result;
        }
    }
}
