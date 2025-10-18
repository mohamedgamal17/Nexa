using MediatR;
using Nexa.Accounting.Application.FundingResources.Factories;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.FundingResources;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.CustomerManagement.Shared.Services;
using Nexa.Integrations.Baas.Abstractions.Contracts.FundingResources;
using Nexa.Integrations.Baas.Abstractions.Services;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Nexa.Integrations.OpenBanking.Abstractions.enums;
namespace Nexa.Accounting.Application.Tokens.Commands.CompleteLinkToken
{
    public class CompleteLinkTokenCommandHandler : IApplicationRequestHandler<CompleteLinkTokenCommand, BankAccountDto>
    {
        private readonly ICustomerService _customerService;
        private readonly IBaasFundingResourceService _baasFundingResourceService;
        private readonly IBankingTokenService _bankingTokenService;
        private readonly IAccountingRepository<BankAccount> _bankAccountRepository;
        private readonly IBankAccountResponseFactory _bankAccountResponseFactory;
        private readonly ISecurityContext _securityContext;

        public CompleteLinkTokenCommandHandler(ICustomerService customerService, IBaasFundingResourceService baasFundingResourceService, IBankingTokenService bankingTokenService, IAccountingRepository<BankAccount> bankAccountRepository, IBankAccountResponseFactory bankAccountResponseFactory, ISecurityContext securityContext)
        {
            _customerService = customerService;
            _baasFundingResourceService = baasFundingResourceService;
            _bankingTokenService = bankingTokenService;
            _bankAccountRepository = bankAccountRepository;
            _bankAccountResponseFactory = bankAccountResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<BankAccountDto>> Handle(CompleteLinkTokenCommand request, CancellationToken cancellationToken)
        {

            var userId = _securityContext.User!.Id;

            var customer = await _customerService.GetCustomerByUserId(userId);

            if (customer == null)
            {
                return new EntityNotFoundException(CustomerErrorConsts.CustomerNotExist);
            }

            if (customer.State != VerificationState.Verified)
            {
                return new BusinessLogicException(CustomerErrorConsts.CustomerNotVerified);
            }

            var processorTokenRequest = new TokenProcessReqeust
            {
                ClientUserId = customer.FintechCustomerId!,
                Token = request.Token,
                Provider = ProcessorProvider.Stripe
            };

            var processorTokenResult = await _bankingTokenService.ProcessTokenAsync(processorTokenRequest);

            if (processorTokenResult.IsFailure)
            {
                return processorTokenResult.Exception!;

            }
            var processorToken = processorTokenResult.Value!;

            var externalBankAccountRequest = new BaasBankAccountCreateRequest { Token = processorToken.Token };

            var baasAccount = await _baasFundingResourceService.CreateBankAccountAsync(customer.FintechCustomerId!, externalBankAccountRequest);


            var bankAccount = new BankAccount(
                    userId,
                    customer.Id,
                    baasAccount.Id,       
                    baasAccount.Country,
                    baasAccount.Currency,
                    baasAccount.AccountNumberLast4,
                    baasAccount.RoutingNumber,
                    baasAccount.HolderName,
                    baasAccount.BankName
                );


            await  _bankAccountRepository.InsertAsync(bankAccount);

            var dto = await _bankAccountResponseFactory.PrepareDto(bankAccount);

            return dto;
        }
    }
}
