using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Nexa.Integrations.OpenBanking.Abstractions.Exceptions;
using Stripe;
using Stripe.FinancialConnections;

namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeBankingService : IBankingTokenService
    {
        private SessionService _sessionService;

        public StripeBankingService()
        {
            _sessionService = new SessionService();
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
                    "balances"
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

        public async Task<Result<ProcessorToken>> ProcessTokenAsync(TokenProcessReqeust reqeust, CancellationToken cancellationToken = default)
        {
            try
            {
                var session = await _sessionService.GetAsync(reqeust.Token);

                if (session.Accounts.Count() <= 0)
                {
                    return new Result<ProcessorToken>(new OpenBankingExcetpion("Current session is incomplete"));
                }

                var account = session.Accounts.First();

                var token = new ProcessorToken { Token = account.Id };

                return token;

            }
            catch(StripeException exception) when(exception.StripeResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new Result<ProcessorToken>(new OpenBankingExcetpion("session is not exist"));
            }
            
        }
    }
}
