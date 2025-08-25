using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.CustomerManagement.Shared.Services;
using Nexa.Integrations.Baas.Abstractions.Contracts.FundingResources;
using Nexa.Integrations.Baas.Abstractions.Services;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Nexa.Integrations.OpenBanking.Abstractions.enums;
namespace Nexa.Accounting.Application.Tokens.Commands.CompleteLinkToken
{
    public class CompleteLinkTokenCommandHandler : IApplicationRequestHandler<CompleteLinkTokenCommand, Unit>
    {
        private readonly ICustomerService _customerService;
        private readonly IBaasFundingResourceService _baasFundingResourceService;
        private readonly IBankingTokenService _bankingTokenService;
        private readonly ISecurityContext _securityContext;
        public CompleteLinkTokenCommandHandler(ICustomerService customerService, IBaasFundingResourceService baasFundingResourceService, IBankingTokenService bankingTokenService, ISecurityContext securityContext)
        {
            _customerService = customerService;
            _baasFundingResourceService = baasFundingResourceService;
            _bankingTokenService = bankingTokenService;
            _securityContext = securityContext;
        }

        public async Task<Result<Unit>> Handle(CompleteLinkTokenCommand request, CancellationToken cancellationToken)
        {

            var userId = _securityContext.User!.Id;

            var customer = await _customerService.GetCustomerByUserId(userId);

            if (customer == null)
            {
                return new Result<Unit>(new BusinessLogicException("User should complete and validate customer information before processding in linking bank accounts"));
            }

            if (customer.State != VerificationState.Verified)
            {
                return new Result<Unit>(new BusinessLogicException("Customer should be verified first before linking bank account."));
            }

            var exchangedToken = await _bankingTokenService.ExchangeTokenAsync(request.Token);

            var processorTokenRequest = new ProcessorTokenCreateReqeust
            {
                AccessToken = exchangedToken.AccessToken,
                Provider = ProcessorProvider.Stripe
            };

            var processorToken = await _bankingTokenService.CreateProcessorToken(processorTokenRequest);

            var externalBankAccountRequest = new BaasBankAccountCreateRequest { Token = processorToken.Token };

            var baasAccount = await _baasFundingResourceService.CreateBankAccountAsync(customer.FintechCustomerId!, externalBankAccountRequest);

            return Unit.Value;
        }
    }
}
