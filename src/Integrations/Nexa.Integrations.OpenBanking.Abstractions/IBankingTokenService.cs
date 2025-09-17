using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;

namespace Nexa.Integrations.OpenBanking.Abstractions
{
    public interface IBankingTokenService
    {
        Task<Result<LinkToken>> CreateTokenAsync(TokenCreateRequest request, CancellationToken cancellationToken = default);

        Task<Result<ProcessorToken>> ProcessTokenAsync(TokenProcessReqeust reqeust, CancellationToken cancellationToken = default);
    }
}
