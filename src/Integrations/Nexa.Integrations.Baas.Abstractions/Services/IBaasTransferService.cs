using Nexa.Integrations.Baas.Abstractions.Contracts.Transfers;

namespace Nexa.Integrations.Baas.Abstractions.Services
{
    public interface IBaasTransferService
    {
        Task<BaasBankTransfer> Deposit(BankTransferRequest request , CancellationToken cancellationToken = default);

        Task<BaasNetworkTransfer> NetworkTransfer(NetworkTransferRequest request, CancellationToken cancellationToken = default);

        Task<BaasBankTransfer> Withdraw(BankTransferRequest request, CancellationToken cancellationToken = default);
    }
}
