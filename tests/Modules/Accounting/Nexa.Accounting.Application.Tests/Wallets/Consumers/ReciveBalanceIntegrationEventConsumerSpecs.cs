using FluentAssertions;
using Nexa.Accounting.Application.Tests.Assertions;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Shared.Events;

namespace Nexa.Accounting.Application.Tests.Wallets.Consumers
{
    [TestFixture]
    public class When_recive_balance_integration_event_consumed : WalletTestFixture
    {
        [Test]
        public async Task Should_add_new_balance_to_wallet_and_publish_recive_balance_completed_event()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeWallet = await CreateWalletAsync(userId);

            var message = new ReciveBalanceIntegrationEvent
            {
                TransferId = Guid.NewGuid().ToString(),
                TransferNumber = Guid.NewGuid().ToString(),
                FundingResourceId = Guid.NewGuid().ToString(),
                WalletId = fakeWallet.Id,
                Amount = 50,
                CompletedAt = DateTime.UtcNow
            };

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<ReciveBalanceIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<TransferCompletedIntegrationEvent>());

            var wallet = await WalletRepository.SingleAsync(x => x.Id == fakeWallet.Id);

            var ledgerEntry = await LedgerEntryRepository.SingleOrDefaultAsync(x => x.TransactionId == message.TransferId);

            wallet.Balance.Should().Be(message.Amount);

            ledgerEntry.Should().NotBeNull();

            ledgerEntry!.AssertLedgerEntry(
                    wallet.Id,
                    message.TransferId,
                    message.Amount,
                    TransferType.Bank,
                    TransferDirection.Credit
                );

        }
    }
}
