using FluentAssertions;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Application.Transfers.Consumer;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class When_recive_balance_completed_integration_event_consumed : TransferTestFixture
    {
        [Test]
        public async Task Should_complete_bank_transfer()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeWallet = await CreateWalletAsync(userId);

            var fakeFundingResource = await CreateFundingResourceAsync(userId);

            var fakeTransfer = await CreateProcessBankTransferWithExternalIdAsync(userId, fakeWallet.Id, fakeWallet.Id, 100, TransferDirection.Credit);

            await TestHarness.Start();

            var message = new ReciveBalanceCompletedIntegrationEvent
            {
                TransferId = fakeTransfer.Id,
                WalletId = fakeWallet.Id
            };

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<ReciveBalanceCompletedIntegrationEvent>());

            var transfer = await TransferRepository.SingleAsync(x => x.Id == fakeTransfer.Id);

            transfer.Status.Should().Be(TransferStatus.Completed);
        }
    }
}
