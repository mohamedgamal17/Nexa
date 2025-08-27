using Bogus;
using Nexa.Integrations.Baas.Abstractions.Contracts.FundingResources;
using Nexa.Integrations.Baas.Abstractions.Services;

namespace Nexa.Application.Tests.Providers.Baas
{
    public class FakeBaasFundingResurceService : IBaasFundingResourceService
    {
        private readonly static List<BaasBankAccount> _baasBankAccounts = new List<BaasBankAccount>();
        private readonly Faker _faker = new Faker();
        public Task<BaasBankAccount> CreateBankAccountAsync(string accountId, BaasBankAccountCreateRequest request, CancellationToken cancellationToken = default)
        {
            var response = new BaasBankAccount
            {
                Id = accountId,
                BankName = Guid.NewGuid().ToString(),
                HolderName = Guid.NewGuid().ToString(),
                AccountNumberLast4 = _faker.Finance.Account(4),
                RoutingNumber = _faker.Finance.RoutingNumber(),
                Country = "US",
                Currency = "USD"
            };

            _baasBankAccounts.Add(response);

            return Task.FromResult(response);
        }

        public Task<BaasBankAccount> GetBankAccountAsync(string accountId, string bankAccountId, CancellationToken cancellationToken = default)
        {
            var response = _baasBankAccounts.Single(x => x.Id == bankAccountId);

            return Task.FromResult(response);
        }
    }
}
