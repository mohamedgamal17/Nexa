using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Going.Plaid;
using Going.Plaid.Entity;
using Nexa.Integrations.OpenBanking.Abstractions.enums;
using Going.Plaid.Item;
using Going.Plaid.Processor;
using Going.Plaid.Accounts;
namespace Nexa.Integrations.OpenBanking.Plaid
{
    public class PlaidBankingTokenService : IBankingTokenService
    {
        private readonly PlaidClient _plaidClient;
        public PlaidBankingTokenService(PlaidClient plaidClient)
        { 
            _plaidClient = plaidClient;
        }
        public async Task<LinkToken> CreateLinkTokenAsync(LinkTokenCreateRequest request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new Going.Plaid.Link.LinkTokenCreateRequest
            {
                ClientName = request.ClinetName,
                CountryCodes = request.CountryCodes.Select(MapCountry).ToList(),
                Language = MapLanguage(request.Language),
                Products = new List<Products> { Products.Transactions},
                AccountFilters = new LinkTokenAccountFilters
                {
                    Depository = new DepositoryFilter
                    {
                        AccountSubtypes = new List<DepositoryAccountSubtype>
                        {
                            DepositoryAccountSubtype.Checking,
                            DepositoryAccountSubtype.Savings
                        },
                    },

                    Credit = new CreditFilter
                    {
                        AccountSubtypes = new List<CreditAccountSubtype>
                        {
                            CreditAccountSubtype.CreditCard
                        }
                    }
                },

                User=  new LinkTokenCreateRequestUser
                {
                    ClientUserId = request.User.ClinetUserId!,
                    PhoneNumber = request.User?.PhoneNumber
                },
                RedirectUri = request.RedirectUri 
            };

            var response = await  _plaidClient.LinkTokenCreateAsync(apiRequest);

            var linkToken = new LinkToken
            {
                Token = response.LinkToken,
                Expiration = response.Expiration.DateTime
            };

            return linkToken;
        }


        public async Task<TokenExchange> ExchangeTokenAsync(string publicToken, CancellationToken cancellationToken)
        {
            var apiRequest = new ItemPublicTokenExchangeRequest
            {
                PublicToken = publicToken
            };

            var response = await _plaidClient.ItemPublicTokenExchangeAsync(apiRequest);

            var exchange = new TokenExchange
            {
                AccessToken = response.AccessToken
            };
         
            return exchange;
        }

        public async Task<ProcessorToken> CreateProcessorToken(ProcessorTokenCreateReqeust reqeust, CancellationToken cancellationToken = default)
        {
            var accountsResposnse = await _plaidClient
                .AccountsGetAsync(new AccountsGetRequest { AccessToken = reqeust.AccessToken});


            var depositoryOrCreditAccounts = accountsResposnse.Accounts
                .Where(x => x.Type == AccountType.Depository || x.Type == AccountType.Credit)
                .ToList();

            if(depositoryOrCreditAccounts.Count <= 0)
            {
                throw new InvalidOperationException("User must select one or more depository or credit accounts.");
            }

            var accountId = depositoryOrCreditAccounts.First().AccountId;

            if (reqeust.Provider == ProcessorProvider.Stripe)
            {
                var apiRequest = new ProcessorStripeBankAccountTokenCreateRequest
                {
                    AccessToken = reqeust.AccessToken,
                    AccountId = accountId
                };

                var response = await _plaidClient.ProcessorStripeBankAccountTokenCreateAsync(apiRequest);

                var token = new ProcessorToken
                {
                    Token = response.StripeBankAccountToken
                };

                return token;

            }
            else
            {
                var apiRequest = new ProcessorTokenCreateRequest
                {
                    AccessToken = reqeust.AccessToken,
                    AccountId = accountId,
                    Processor = ProcessorProviderMap(reqeust.Provider)
                };

                var response = await _plaidClient.ProcessorTokenCreateAsync(apiRequest);

                var token = new ProcessorToken
                {
                    Token = response.ProcessorToken
                };

                return token;

            }
        }


        private Language MapLanguage(LanguageIsoCode language)
            => language switch
            {
                LanguageIsoCode.en => Language.English,
                _ => throw new ArgumentOutOfRangeException(nameof(language))
            };

        private CountryCode MapCountry(CountryIsoCode country)
           => country switch
           {
               CountryIsoCode.Us => CountryCode.Us,
               _ => throw new ArgumentOutOfRangeException(nameof(country))
           };

        private ProcessorTokenCreateRequestProcessorEnum ProcessorProviderMap(ProcessorProvider processor)
            => processor switch
            {
                ProcessorProvider.Dowlla => ProcessorTokenCreateRequestProcessorEnum.Dwolla,
                ProcessorProvider.TreasuryPrime => ProcessorTokenCreateRequestProcessorEnum.TreasuryPrime,
                _ => throw new ArgumentOutOfRangeException(nameof(processor))
            };

    }
}
