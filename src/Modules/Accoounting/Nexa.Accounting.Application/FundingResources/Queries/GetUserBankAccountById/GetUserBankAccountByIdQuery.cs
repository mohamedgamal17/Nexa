using Nexa.Accounting.Application.FundingResources.Factories;
using Nexa.Accounting.Domain.FundingResources;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Vogel.BuildingBlocks.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.Accounting.Shared.Consts;

namespace Nexa.Accounting.Application.FundingResources.Queries.GetUserBankAccountById
{
    public class GetUserBankAccountByIdQuery : IQuery<BankAccountDto>
    {
        public string BankAccountId { get; set; }
    }

    public class GetUserBankAccountByIdQueryHandler : IApplicationRequestHandler<GetUserBankAccountByIdQuery, BankAccountDto>
    {

        private readonly IAccountingRepository<BankAccount> _bankAccountRepository;
        private readonly IBankAccountResponseFactory _bankAccountResponseFactory;
        private readonly ISecurityContext _securityContext;

        public GetUserBankAccountByIdQueryHandler(IAccountingRepository<BankAccount> bankAccountRepository, IBankAccountResponseFactory bankAccountResponseFactory, ISecurityContext securityContext)
        {
            _bankAccountRepository = bankAccountRepository;
            _bankAccountResponseFactory = bankAccountResponseFactory;
            _securityContext = securityContext;
        }
        public async Task<Result<BankAccountDto>> Handle(GetUserBankAccountByIdQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var result = await _bankAccountRepository.AsQuerable()
                .Where(x => x.UserId == userId)
                .SingleOrDefaultAsync(x=> x.Id == request.BankAccountId);


            if(result == null)
            {
                return new EntityNotFoundException(BankAccountErrorConsts.BankAccountNotExist);
            }


            return await _bankAccountResponseFactory.PrepareDto(result);
        }
    }
}
