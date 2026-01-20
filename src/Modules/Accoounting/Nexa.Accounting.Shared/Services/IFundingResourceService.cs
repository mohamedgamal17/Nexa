using Nexa.Accounting.Shared.Dtos;

namespace Nexa.Accounting.Shared.Services
{
    public interface IFundingResourceService
    {
        Task<BankAccountDto?> GetFundingResourceById(string id, CancellationToken cancellationToken = default);
        Task<BankAccountDto?> GetById(string id, CancellationToken cancellationToken = default);
    }
}
