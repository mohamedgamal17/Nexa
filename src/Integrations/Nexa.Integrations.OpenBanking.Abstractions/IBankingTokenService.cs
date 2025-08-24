using Nexa.Integrations.OpenBanking.Abstractions.Contracts;

namespace Nexa.Integrations.OpenBanking.Abstractions
{
    public interface IBankingTokenService
    {
        Task<LinkToken> CreateLinkTokenAsync(LinkTokenCreateRequest request, CancellationToken cancellationToken = default);

        Task<TokenExchange> ExchangeTokenAsync(string publicToken, CancellationToken cancellationToken =default);

        Task<ProcessorToken> CreateProcessorToken(ProcessorTokenCreateReqeust reqeust, CancellationToken cancellationToken = default);
    }
}
