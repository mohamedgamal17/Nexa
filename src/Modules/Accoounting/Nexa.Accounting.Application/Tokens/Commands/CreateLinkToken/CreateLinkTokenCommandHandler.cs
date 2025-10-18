using Nexa.Accounting.Application.Tokens.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.CustomerManagement.Shared.Services;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Nexa.Integrations.OpenBanking.Abstractions.enums;

namespace Nexa.Accounting.Application.Tokens.Commands.CreateLinkToken
{
    public class CreateLinkTokenCommandHandler : IApplicationRequestHandler<CreateLinkTokenCommand, BankingTokenDto>
    {
        private readonly ICustomerService _customerService;

        private readonly IBankingTokenService _bankingTokenService;

        private readonly ISecurityContext _securityContext;

        public CreateLinkTokenCommandHandler(ICustomerService customerService,  IBankingTokenService bankingTokenService, ISecurityContext securityContext)
        {
            _customerService = customerService;
            _bankingTokenService = bankingTokenService;
            _securityContext = securityContext;
        }

        public async Task<Result<BankingTokenDto>> Handle(CreateLinkTokenCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerService.GetCustomerByUserId(userId);

            if (customer == null)
            {
                return new EntityNotFoundException(CustomerErrorConsts.CustomerNotExist);
            }

            if (customer.State != VerificationState.Verified)
            {
                return new BusinessLogicException(CustomerErrorConsts.CustomerNotVerified);
            }

            var apiRequest = new TokenCreateRequest
            {
                ClientUserId = customer.FintechCustomerId!,
    
                CountryCodes = new List<CountryIsoCode>
                {
                    CountryIsoCode.Us
                },
                RedirectUri = request.RedirectUri
            };

            var response = await _bankingTokenService.CreateTokenAsync(apiRequest);

            if (response.IsFailure)
            {
                return response.Exception!;
            }

            var dto = new BankingTokenDto
            {
                Token = response.Value!.Token
            };

            return dto;
        }
    }
}
