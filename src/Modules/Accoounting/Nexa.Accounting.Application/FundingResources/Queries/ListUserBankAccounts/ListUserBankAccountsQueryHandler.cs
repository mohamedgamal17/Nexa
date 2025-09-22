using Nexa.Accounting.Application.FundingResources.Factories;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.FundingResources;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.BuildingBlocks.Domain.Results;
using Vogel.BuildingBlocks.EntityFramework.Extensions;

namespace Nexa.Accounting.Application.FundingResources.Queries.ListUserBankAccounts
{
    public class ListUserBankAccountsQueryHandler : IApplicationRequestHandler<ListUserBankAccountsQuery, Paging<BankAccountDto>>
    {
        private readonly IAccountingRepository<BankAccount> _bankAccountRepository;
        private readonly IBankAccountResponseFactory _bankAccountResponseFactory;
        private readonly ISecurityContext _securityContext;

        public ListUserBankAccountsQueryHandler(IAccountingRepository<BankAccount> bankAccountRepository, IBankAccountResponseFactory bankAccountResponseFactory, ISecurityContext securityContext)
        {
            _bankAccountRepository = bankAccountRepository;
            _bankAccountResponseFactory = bankAccountResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<Paging<BankAccountDto>>> Handle(ListUserBankAccountsQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var result = await _bankAccountRepository.AsQuerable()
                .Where(x => x.UserId == userId)
                .ToPaged(request.Skip, request.Length);


            return await _bankAccountResponseFactory.PreparePagingDto(result);
        }
    }
}
