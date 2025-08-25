using Nexa.Integrations.Baas.Abstractions.Contracts.FundingResources;

namespace Nexa.Integrations.Baas.Abstractions.Services
{
    public interface IBaasFundingResourceService
    {
        Task<BaasBankAccount> CreateBankAccountAsync(string accountId, BaasBankAccountCreateRequest request, CancellationToken cancellationToken = default);

        Task<BaasBankAccount> GetBankAccountAsync(string accountId, string bankAccountId, CancellationToken cancellationToken = default);
    }
}
