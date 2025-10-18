using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Consts;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Stripe;
using Stripe.FinancialConnections;
namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeBankingService : IBankingTokenService
    {
        private SessionService _sessionService;
        private TokenService _tokenService;
        public StripeBankingService()
        {
            _sessionService = new SessionService();
            _tokenService = new TokenService();
        }
        public async Task<Result<LinkToken>> CreateTokenAsync(TokenCreateRequest request, CancellationToken cancellationToken = default)
        {

            var apiRequest = new SessionCreateOptions
            {
                AccountHolder = new SessionAccountHolderOptions
                {
                    Type = "account",
                    Account = request.ClientUserId,

                },
                Permissions = new List<string>
                {
                    "payment_method",
                    "balances",
                    "transactions",
                    "ownership"
                },

                Filters = new SessionFiltersOptions
                {
                    AccountSubcategories = new List<string>
                    {
                        "checking",
                        "savings"
                    },
                    Countries = request.CountryCodes.Select(x => x.ToString().ToLower()).ToList()

                },

                ReturnUrl = request.RedirectUri,
                
            };


            var response = await _sessionService.CreateAsync(apiRequest);


            var linkToken = new LinkToken { Token = response.ClientSecret };

            return linkToken;
        }

        public async Task<Result<ProcessorToken>> ProcessTokenAsync(TokenProcessReqeust request, CancellationToken cancellationToken = default)
        {
            try
            {
                var requestOptions = new RequestOptions { StripeAccount = request.ClientUserId };

                var bankToken = await _tokenService.GetAsync(request.Token);

                if (bankToken.Used)
                {
                    return new BusinessLogicException(OpenBankingErrorConsts.InvalidBankToken);
                }

                var result = new ProcessorToken
                {
                    Token = bankToken.Id
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
