using Nexa.Accounting.Application.Tokens.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Nexa.Integrations.OpenBanking.Abstractions.enums;

namespace Nexa.Accounting.Application.Tokens.Commands.CreateLinkToken
{
    public class CreateLinkTokenCommandHandler : IApplicationRequestHandler<CreateLinkTokenCommand, BankingTokenDto>
    {
        private readonly IBankingTokenService _bankingTokenService;

        private readonly ISecurityContext _securityContext;

        public CreateLinkTokenCommandHandler(IBankingTokenService bankingTokenService, ISecurityContext securityContext)
        {
            _bankingTokenService = bankingTokenService;
            _securityContext = securityContext;
        }

        public async Task<Result<BankingTokenDto>> Handle(CreateLinkTokenCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

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

            var dto = new BankingTokenDto
            {
                Token = response.Token
            };

            return dto;
        }
    }
}
