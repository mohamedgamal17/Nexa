using FluentAssertions;
using Nexa.Accounting.Shared.Events;

namespace Nexa.Accounting.Application.Tests.Wallets.Consumers
{
    [TestFixture]
    public class When_transfer_funds_integration_event_consumed : WalletTestFixture 
    {

        [Test]
        public async Task Should_depit_wallet_balance_and_publish_transfer_completed_event()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeWallet = await CreateWalletWithReservedBalanceAsync(userId, 200, 100);

            var fakeBankAccount = await CreateBankAccountAsync(userId);

            var @event = new TransferFundsIntegrationEvent
            {
                TransferId = Guid.NewGuid().ToString(),
                TransferNumber = Guid.NewGuid().ToString(),
                FundingResourceId = fakeBankAccount.Id,
                WalletId = fakeWallet.Id,
                Amount = 100,
                CompletedAt = DateTime.UtcNow
            };

            await TestHarness.Bus.Publish(@event);

            Assert.That(await TestHarness.Consumed.Any<TransferFundsIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<TransferCompletedIntegrationEvent>());

            var wallet = await WalletRepository.SingleAsync(x => x.Id == fakeWallet.Id);

            wallet.ReservedBalance.Should().Be(0);

            wallet.Balance.Should().Be(100);
        }
    }
}
