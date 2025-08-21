using Nexa.Integrations.Baas.Abstractions.Contracts.Wallets;

namespace Nexa.Integrations.Baas.Abstractions.Services
{
    public interface IBaasWalletService
    {
        Task<BaasWallet> CreateWalletAsync(string clientId, CancellationToken cancellationToken = default);
    }
}
