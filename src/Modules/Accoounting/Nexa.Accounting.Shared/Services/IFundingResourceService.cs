using Nexa.Accounting.Shared.Dtos;

namespace Nexa.Accounting.Shared.Services
{
    public interface IFundingResourceService
    {
        Task<List<BankAccountDto>> ListByIds(List<string> ids, CancellationToken cancellationToken = default);

        Task<BankAccountDto?> GetById(string id, CancellationToken cancellationToken = default);
    }
}
