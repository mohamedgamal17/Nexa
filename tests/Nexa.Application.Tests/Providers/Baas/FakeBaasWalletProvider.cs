using Nexa.Integrations.Baas.Abstractions.Contracts.Wallets;
using Nexa.Integrations.Baas.Abstractions.Services;

namespace Nexa.Application.Tests.Providers.Baas
{
    public class FakeBaasWalletProvider : IBaasWalletService
    {
        private readonly static List<BaasWallet> _wallets = new List<BaasWallet>();
        public Task<BaasWallet> CreateWalletAsync(string clientId, CancellationToken cancellationToken = default)
        {
            var wallet = new BaasWallet
            {
                Id = Guid.NewGuid().ToString(),
                Balance = 0
            };

            return Task.FromResult(wallet);
        }
    }
}
