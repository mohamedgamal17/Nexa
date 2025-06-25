using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Shared.Dtos;
using Nexa.Transactions.Application.Tests.Fakers;
using Nexa.Transactions.Domain;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Enums;
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

        protected override async Task InitializeAsync(IServiceProvider services)
        {
            await base.InitializeAsync(services);

            await TestHarness.Start();
        }

        protected override async Task ShutdownAsync(IServiceProvider services)
        {
            await base.ShutdownAsync(services);

            await TestHarness.Stop();
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

        public async Task<NetworkTransfer> CreateRandomNetworkTransfer(TransferStatus status = TransferStatus.Pending)
        {
            var senderWallet = await CreateWalletAsync(balance: 1000);
            var reciverWallet = await CreateWalletAsync();
            return await CreateNetworkTransferAsync(senderWallet.Id, reciverWallet.Id, 500 , status);
        }

        public async Task<NetworkTransfer> CreateNetworkTransferAsync(string walletId , string reciverId , decimal amount, TransferStatus status)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var transferRepository = sp.GetRequiredService<ITransactionRepository<NetworkTransfer>>();
                var networkTransfer = new NetworkTransfer(walletId, reciverId, Guid.NewGuid().ToString(), amount,status);

                return await transferRepository.InsertAsync(networkTransfer);
            });
        }

        public async Task<BankTransfer> CreateRandomBankTransferAsync(BankTransferType bankTransferType , TransferDirection direction , TransferStatus status = TransferStatus.Pending)
        {
            var wallet = await CreateWalletAsync(balance: 1000);

            return await CreateBankTransferAsync(wallet.Id, bankTransferType, direction, 500, status);
        }
         
        public async Task<BankTransfer> CreateBankTransferAsync(string walletId ,BankTransferType bankTransferType  , TransferDirection direction,decimal amount , TransferStatus status )
        {
            return await WithScopeAsync(async (sp) =>
            {
                var transferRepository = sp.GetRequiredService<ITransactionRepository<BankTransfer>>();

                var bankTransfer = new BankTransfer(walletId, Guid.NewGuid().ToString(), amount, Guid.NewGuid().ToString(), direction, bankTransferType, status);

                return await transferRepository.InsertAsync(bankTransfer);
            });
        }
    }
}
