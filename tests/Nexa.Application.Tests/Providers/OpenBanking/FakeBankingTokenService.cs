using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Integrations.OpenBanking.Abstractions;
using Nexa.Integrations.OpenBanking.Abstractions.Contracts;
namespace Nexa.Application.Tests.Providers.OpenBanking
{
    public class FakeBankingTokenService : IBankingTokenService
    {

        public async Task<Result<LinkToken>> CreateTokenAsync(TokenCreateRequest request, CancellationToken cancellationToken = default)
        {

            var linkToken = new LinkToken { Token = Guid.NewGuid().ToString(), Expiration = DateTime.Now.AddDays(1) };

            return await Task.FromResult(linkToken);
        }

        public async Task<Result<ProcessorToken>> ProcessTokenAsync(TokenProcessReqeust reqeust, CancellationToken cancellationToken = default)
        {
            var token = new ProcessorToken { Token = Guid.NewGuid().ToString() };

            return await Task.FromResult(token);
    
        }
    }
}
