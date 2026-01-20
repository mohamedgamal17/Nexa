using Nexa.Accounting.Shared.Dtos;
using Nexa.Accounting.Shared.Services;

namespace Nexa.Transactions.Application.Tests.Fakers
{
    public class FakeFundingResourceService : IFundingResourceService
    {
        private static List<BankAccountDto> _db = new List<BankAccountDto>();
        public Task<BankAccountDto?> GetById(string id, CancellationToken cancellationToken = default)
        {
            var bankAccount = _db.SingleOrDefault(x => x.Id == id);

            return Task.FromResult(bankAccount);
        }

        public Task<List<BankAccountDto>> ListByIds(List<string> ids, CancellationToken cancellationToken)
        {
            var result = _db.Where(x => ids.Contains(x.Id)).ToList();

            return Task.FromResult(result);
        }

        public  Task<BankAccountDto> AddFundingResource(BankAccountDto bankAccountDto , CancellationToken cancellationToken = default)
        {
            _db.Add(bankAccountDto);

            return Task.FromResult(bankAccountDto);
        }

      
    }
}
