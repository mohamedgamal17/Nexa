using Nexa.Accounting.Application.Tokens.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.CustomerManagement.Shared.Services;
using Nexa.Integrations.Baas.Abstractions.Services;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Nexa.Integrations.OpenBanking.Abstractions.enums;

namespace Nexa.Accounting.Application.Tokens.Commands.CreateLinkToken
{
    public class CreateLinkTokenCommandHandler : IApplicationRequestHandler<CreateLinkTokenCommand, BankingTokenDto>
    {
        private readonly ICustomerService _customerService;

        private readonly IBaasFundingResourceService _baasFundingResourceService;

        private readonly IBankingTokenService _bankingTokenService;

        private readonly ISecurityContext _securityContext;

        public CreateLinkTokenCommandHandler(ICustomerService customerService, IBaasFundingResourceService baasFundingResourceService, IBankingTokenService bankingTokenService, ISecurityContext securityContext)
        {
            _customerService = customerService;
            _baasFundingResourceService = baasFundingResourceService;
            _bankingTokenService = bankingTokenService;
            _securityContext = securityContext;
        }

        public async Task<Result<BankingTokenDto>> Handle(CreateLinkTokenCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerService.GetCustomerByUserId(userId);

            if(customer == null)
            {
                return new Result<BankingTokenDto>(new BusinessLogicException("User should complete and validate customer information before processding in linking bank accounts"));
            }

            if(customer.State != VerificationState.Verified)
            {
                return new Result<BankingTokenDto>(new BusinessLogicException("Customer should be verified first before linking bank account."));
            }

            var apiRequest = new LinkTokenCreateRequest
            {
                ClinetName = "Nexa Wallet",
                User = new LinkTokenUser
                {
                    ClinetUserId = userId,
                },
                CountryCodes = new List<CountryIsoCode>
                {
                    CountryIsoCode.Us
                },
                Language = LanguageIsoCode.en,
                RedirectUri = request.RedirectUri
            };

            var response = await _bankingTokenService.CreateLinkTokenAsync(apiRequest);

            if (response.IsFailure)
            {
                return new Result<BankingTokenDto>(new BusinessLogicException(response.Exception!.Message));
            }

            var dto = new BankingTokenDto
            {
                Token = response.Value!.Token
            };

            return dto;
        }
    }
}
