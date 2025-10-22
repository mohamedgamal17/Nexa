using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Consts;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Stripe;
namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeBankingService : IBankingTokenService
    {
        private SetupIntentService _setupIntentService;
        public StripeBankingService()
        {
            _setupIntentService = new SetupIntentService();
        }
        public async Task<Result<LinkToken>> CreateTokenAsync(TokenCreateRequest request, CancellationToken cancellationToken = default)
        {

            var apiRequest = new SetupIntentCreateOptions
            {
                PaymentMethodTypes = ["us_bank_account"],
                FlowDirections = ["inbound", "outbound"],
                AttachToSelf =  true,
                PaymentMethodOptions = new SetupIntentPaymentMethodOptionsOptions
                {
                    UsBankAccount = new SetupIntentPaymentMethodOptionsUsBankAccountOptions
                    {
                        FinancialConnections = new SetupIntentPaymentMethodOptionsUsBankAccountFinancialConnectionsOptions
                        {
                            Permissions = ["payment_method", "balances", "transactions", "ownership"],
                            Filters = new SetupIntentPaymentMethodOptionsUsBankAccountFinancialConnectionsFiltersOptions
                            {
                                AccountSubcategories = ["checking", "savings"],
                            }
                        },

                    }
                }, 
                Usage = "off_session",
                                            
            };


            var response = await _setupIntentService.CreateAsync(apiRequest);

            var linkToken = new LinkToken { Token = response.ClientSecret };

            return linkToken;
        }

        public async Task<Result<ProcessorToken>> ProcessTokenAsync(TokenProcessReqeust request, CancellationToken cancellationToken = default)
        {
            try
            {
                var setupIntent = await _setupIntentService.GetAsync(request.Token );

                if(setupIntent.Status != "succeeded")
                {
                    return new BusinessLogicException(OpenBankingErrorConsts.IncompleteBankToken);
                }

                var result = new ProcessorToken
                {
                    Token = setupIntent.PaymentMethodId
                };

                return result;

            }
            catch(StripeException exception)
                when(exception.StripeError.Code == "invalid_request_error")
            {
                return new EntityNotFoundException(OpenBankingErrorConsts.BankTokenNotExist);
            }          
        }
    }
}
