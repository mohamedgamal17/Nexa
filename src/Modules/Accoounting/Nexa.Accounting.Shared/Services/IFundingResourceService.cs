using Nexa.Accounting.Application.FundingResources.Dtos;

namespace Nexa.Accounting.Shared.Services
{
    public interface IFundingResourceService
    {
        Task<BankAccountDto?> GetFundingResourceById(string id, CancellationToken cancellationToken = default);
    }
}
