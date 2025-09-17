using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Going.Plaid;
using Going.Plaid.Entity;
using Nexa.Integrations.OpenBanking.Abstractions.enums;
using Going.Plaid.Item;
using Going.Plaid.Processor;
using Going.Plaid.Accounts;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Integrations.OpenBanking.Abstractions.Exceptions;
namespace Nexa.Integrations.OpenBanking.Plaid
{
    public class PlaidBankingTokenService : IBankingTokenService
    {
        private readonly PlaidClient _plaidClient;
        public PlaidBankingTokenService(PlaidClient plaidClient)
        { 
            _plaidClient = plaidClient;
        }
        public async Task<Result<LinkToken>> CreateTokenAsync(TokenCreateRequest request, CancellationToken cancellationToken = default)
        {
            var apiRequest = new Going.Plaid.Link.LinkTokenCreateRequest
            {
                ClientName = "Nexa Digital Wallet App",
                CountryCodes = request.CountryCodes.Select(MapCountry).ToList(),
                Language =Language.English,
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
                    ClientUserId = request.ClientUserId
                },
                RedirectUri = request.RedirectUri 
            };

            var response = await  _plaidClient.LinkTokenCreateAsync(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                return new Result<LinkToken>(new OpenBankingExcetpion(response.Error!.ErrorMessage));
            }

            var linkToken = new LinkToken
            {
                Token = response.LinkToken,
                Expiration = response.Expiration.DateTime
            };

            return linkToken;
        }


   

        public async Task<Result<ProcessorToken>> ProcessTokenAsync(TokenProcessReqeust reqeust, CancellationToken cancellationToken = default)
        {
            var exchangeResult = await ExchangeTokenAsync(reqeust.Token,cancellationToken);

            if (exchangeResult.IsFailure)
            {
                return new Result<ProcessorToken>(exchangeResult.Exception!);
            }

            var accessToken = exchangeResult.Value;

            var accountsResposnse = await _plaidClient
                .AccountsGetAsync(new AccountsGetRequest { AccessToken = accessToken });


            if (!accountsResposnse.IsSuccessStatusCode)
            {
                return new Result<ProcessorToken>(new OpenBankingExcetpion(accountsResposnse.Error!.ErrorMessage));

            }

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
                    AccessToken = accessToken,
                    AccountId = accountId
                };

                var response = await _plaidClient.ProcessorStripeBankAccountTokenCreateAsync(apiRequest);

                if (!response.IsSuccessStatusCode)
                {
                    return new Result<ProcessorToken>(new OpenBankingExcetpion(response.Error!.ErrorMessage));
                }

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
                    AccessToken = reqeust.Token,
                    AccountId = accountId,
                    Processor = ProcessorProviderMap(reqeust.Provider)
                };

                var response = await _plaidClient.ProcessorTokenCreateAsync(apiRequest);

                if (!response.IsSuccessStatusCode)
                {
                    return new Result<ProcessorToken>(new OpenBankingExcetpion(response.Error!.ErrorMessage));
                }

                var token = new ProcessorToken
                {
                    Token = response.ProcessorToken
                };

                return token;

            }
        }

        private async Task<Result<string>> ExchangeTokenAsync(string publicToken, CancellationToken cancellationToken = default)
        {
            var apiRequest = new ItemPublicTokenExchangeRequest
            {
                PublicToken = publicToken
            };

            var response = await _plaidClient.ItemPublicTokenExchangeAsync(apiRequest);

            if (!response.IsSuccessStatusCode)
            {
                return new Result<string>(new OpenBankingExcetpion(response.Error!.ErrorMessage));
            }


            return response.AccessToken;
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
