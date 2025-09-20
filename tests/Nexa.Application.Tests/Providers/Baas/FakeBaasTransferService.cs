using Nexa.Integrations.Baas.Abstractions.Contracts.Transfers;
using Nexa.Integrations.Baas.Abstractions.Services;

namespace Nexa.Application.Tests.Providers.Baas
{
    public class FakeBaasTransferService : IBaasTransferService
    {
        public Task<BaasDepositTransfer> Deposit(DepositTransferRequest request, CancellationToken cancellationToken = default)
        {
            var response = new BaasDepositTransfer
            {
                Id = Guid.NewGuid().ToString(),
                WalletId = request.WalletId,
                FundingResource = request.FundingResourceId,
                Amount = request.Amount
            };

            return Task.FromResult(response);
        }
    }
}
