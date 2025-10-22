using Nexa.Integrations.Baas.Abstractions.Contracts.Transfers;
using Nexa.Integrations.Baas.Abstractions.Services;

namespace Nexa.Application.Tests.Providers.Baas
{
    public class FakeBaasTransferService : IBaasTransferService
    {
        public Task<BaasBankTransfer> Deposit(BankTransferRequest request, CancellationToken cancellationToken = default)
        {
            var response = new BaasBankTransfer
            {
                Id = Guid.NewGuid().ToString(),
                WalletId = request.WalletId,
                FundingResourceId = request.FundingResourceId,
                Amount = request.Amount
            };

            return Task.FromResult(response);
        }
        public Task<BaasBankTransfer> Withdraw(BankTransferRequest request, CancellationToken cancellationToken = default)
        {
            var response = new BaasBankTransfer
            {
                Id = Guid.NewGuid().ToString(),
                WalletId = request.WalletId,
                FundingResourceId = request.FundingResourceId,
                Amount = request.Amount
            };

            return Task.FromResult(response);
        }
    }
}
