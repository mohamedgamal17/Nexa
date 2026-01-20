using Microsoft.EntityFrameworkCore;
using Nexa.Accounting.Application.FundingResources.Factories;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.FundingResources;
using Nexa.Accounting.Shared.Dtos;
using Nexa.Accounting.Shared.Services;

namespace Nexa.Accounting.Application.FundingResources.Services
{
    public class FundingResourceService : IFundingResourceService
    {
        private readonly IAccountingRepository<BankAccount> _bankAccountRepository;
        private readonly IBankAccountResponseFactory _bankAccountResponseFactory;

        public FundingResourceService(IAccountingRepository<BankAccount> bankAccountRepository, IBankAccountResponseFactory bankAccountResponseFactory)
        {
            _bankAccountRepository = bankAccountRepository;
            _bankAccountResponseFactory = bankAccountResponseFactory;
        }
        public async Task<List<BankAccountDto>> ListByIds(List<string> ids, CancellationToken cancellationToken)
        {
            var banksAccounts = await _bankAccountRepository.AsQuerable()
                .Where(x => ids.Contains(x.Id))
                .ToListAsync();

            return await _bankAccountResponseFactory.PrepareListDto(banksAccounts);
        }
        public async Task<BankAccountDto?> GetById(string id, CancellationToken cancellationToken = default)
        {
            var bankAccount = await _bankAccountRepository.SingleOrDefaultAsync(x => x.Id == id);

            if (bankAccount == null)
                return default;

            return await _bankAccountResponseFactory.PrepareDto(bankAccount);
        }

        
    }
}
