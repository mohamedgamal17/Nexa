using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Shared.Dtos;
using Nexa.Transactions.Application.Tests.Fakers;
using Nexa.Transactions.Domain.Transfers;
namespace Nexa.Transactions.Application.Tests.Transfers
{
    public class TransferTestFixture : TransactionsTestFixture
    {
        protected FakeWalletService WalletService { get; }
        protected ITransferRepository TransferRepository { get; }

        public TransferTestFixture()
        {
            WalletService = ServiceProvider.GetRequiredService<FakeWalletService>();
            TransferRepository = ServiceProvider.GetRequiredService<ITransferRepository>();
        }
        public async Task<WalletDto> CreateWalletAsync(string? userId = null, decimal balance = 0)
        {
            var wallet = new WalletDto
            {
                Id =  Guid.NewGuid().ToString(),
                UserId = userId ?? Guid.NewGuid().ToString(),
                Balance = balance,
                Number = Guid.NewGuid().ToString()
            };

            return await WalletService.AddWalletAsync(wallet);
        }
    }
}
