using MediatR;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
using Nexa.Integrations.OpenBanking.Abstractions.enums;

namespace Nexa.Accounting.Application.Tokens.Commands.CompleteLinkToken
{
    public class CompleteLinkTokenCommandHandler : IApplicationRequestHandler<CompleteLinkTokenCommand, Unit>
    {
        private readonly IBankingTokenService _bankingTokenService;

        public CompleteLinkTokenCommandHandler(IBankingTokenService bankingTokenService)
        {
            _bankingTokenService = bankingTokenService;
        }

        public async Task<Result<Unit>> Handle(CompleteLinkTokenCommand request, CancellationToken cancellationToken)
        {
            var exchangedToken = await _bankingTokenService.ExchangeTokenAsync(request.Token);

            var processorTokenRequest = new ProcessorTokenCreateReqeust
            {
                AccessToken = exchangedToken.AccessToken,
                Provider = ProcessorProvider.Stripe
            };

            var processorToken = await _bankingTokenService.CreateProcessorToken(processorTokenRequest);

            return Unit.Value;
        }
    }
}
