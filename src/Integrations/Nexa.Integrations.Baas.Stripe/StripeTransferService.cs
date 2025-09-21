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
        private readonly OutboundPaymentService _outboundPaymentService;
        private readonly OutboundTransferService _outboundTransferService;
        public StripeTransferService()
        {
            _inboundTransferService = new InboundTransferService();
            _setupIntentService = new SetupIntentService();
            _outboundPaymentService = new OutboundPaymentService();
            _outboundTransferService = new OutboundTransferService();
        }
        public async Task<BaasBankTransfer> Deposit(BankTransferRequest request, CancellationToken cancellationToken = default)
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
                    { StripeMetaDataConsts.ClientTransferId , request.ClinetTransferId }
                }
            };

            var response = await _inboundTransferService.CreateAsync(inboundTransferRequest, requestOptions);

            var result = new BaasBankTransfer
            {
                Id = response.Id,
                WalletId = response.FinancialAccount,
                Amount = response.Amount / 100,
                FundingResourceId = response.OriginPaymentMethod
            };


            return result;
        }

        public async Task<BaasNetworkTransfer> NetworkTransfer(NetworkTransferRequest request, CancellationToken cancellationToken = default)
        {
            var requestOptions = new RequestOptions { StripeAccount = request.SenderAccountId };

            var options = new OutboundPaymentCreateOptions
            {
                Currency = "usd",
                FinancialAccount = request.SenderWalletId,
                DestinationPaymentMethodData = new OutboundPaymentDestinationPaymentMethodDataOptions
                {
                    Type = "financial_account",
                    FinancialAccount = request.ReciverWalletId
                },
                Amount = (long)request.Amount * 100,
                Metadata = new Dictionary<string, string>
                {
                    { StripeMetaDataConsts.ClientTransferId , request.ClientTransferId }
                }
            };

            var response = await _outboundPaymentService.CreateAsync(options, requestOptions);

            var result = new BaasNetworkTransfer
            {
                Id = response.Id,
                SenderWalletId = response.FinancialAccount,
                ReciverWalletId = response.DestinationPaymentMethodDetails!.FinancialAccount.Id,
                Amount = response.Amount / 100
            };

            return result;
        }

        public async Task<BaasBankTransfer> Withdraw(BankTransferRequest request, CancellationToken cancellationToken = default)
        {
            var options = new OutboundTransferCreateOptions
            {
                Amount = 5 * 100,
                Currency = "usd",
                FinancialAccount = request.WalletId,
                DestinationPaymentMethod = request.FundingResourceId,
                Metadata = new Dictionary<string, string>
                {
                    {StripeMetaDataConsts.ClientTransferId , request.ClinetTransferId }
                }
            };

            var requestOptions = new RequestOptions { StripeAccount = request.AccountId };

            var response = await _outboundTransferService.CreateAsync(options, requestOptions);

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
