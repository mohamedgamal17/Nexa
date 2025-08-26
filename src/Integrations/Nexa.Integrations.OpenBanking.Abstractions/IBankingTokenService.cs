using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;

namespace Nexa.Integrations.OpenBanking.Abstractions
{
    public interface IBankingTokenService
    {
        Task<Result<LinkToken>> CreateLinkTokenAsync(LinkTokenCreateRequest request, CancellationToken cancellationToken = default);

        Task<Result<TokenExchange>> ExchangeTokenAsync(string publicToken, CancellationToken cancellationToken =default);

        Task<Result<ProcessorToken>> CreateProcessorToken(ProcessorTokenCreateReqeust reqeust, CancellationToken cancellationToken = default);
    }
}
