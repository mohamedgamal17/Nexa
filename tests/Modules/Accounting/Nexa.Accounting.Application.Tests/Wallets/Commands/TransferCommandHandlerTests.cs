using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Wallets.Commands.Transfer;
using Nexa.Accounting.Application.Wallets.Services;
using Nexa.Accounting.Domain.Transactions;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Application.Tests.Extensions;

namespace Nexa.Accounting.Application.Tests.Wallets.Commands
{
    [TestFixture]
    public class TransferCommandHandlerTests : AccountingTestFixture
    {
        protected IWalletRepository WalletRepository { get; }

        protected ITransactionRepository TransactionRepository { get; }
        protected IWalletNumberGeneratorService WalletNumberGeneratorService { get; }

        public TransferCommandHandlerTests()
        {
            WalletRepository = ServiceProvider.GetRequiredService<IWalletRepository>();
            TransactionRepository = ServiceProvider.GetRequiredService<ITransactionRepository>();
            WalletNumberGeneratorService = ServiceProvider.GetRequiredService<IWalletNumberGeneratorService>();
        }

        [Test]
        public async Task Should_transfer_balance_from_user_wallet_to_another_wallet()
        {
            AuthenticationService.Login();

            var userId = AuthenticationService.GetCurrentUser()!.Id;

            var senderWallet = await CreateFakeWallet(userId, 1000);

            var reciverWallet = await CreateFakeWallet();

            var command = new TransferCommand
            {
                SenderId = senderWallet.Id,
                ReciverId = reciverWallet.Id,
                Amount = 500
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var transaction = await TransactionRepository.FindByIdAsync(result.Value!.Id);

            transaction.Should().NotBeNull();
        }

        private async Task<Wallet> CreateFakeWallet(string? userId = null , decimal balance = 0)
        {
            var walletNumber =  WalletNumberGeneratorService.Generate();

            var wallet = new Wallet(walletNumber, userId ?? Guid.NewGuid().ToString(), balance);

            return await WalletRepository.InsertAsync(wallet);
        }
    }
}
