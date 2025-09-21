using FluentAssertions;
using MassTransit.Testing;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class When_transfer_completed_integration_event_consumed : TransferTestFixture
    {
        [Test]
        public async Task Should_update_transfer_status_to_complete()
        {

            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var senderWallet = await CreateWalletAsync(userId, 1000);

            var reciverWallet = await CreateWalletAsync(userId, 1000);

            var networkTransfer = await CreateProcessNetworkTransferAsync(userId, senderWallet.Id, reciverWallet.Id, 500);

            await TestHarness.Start();

            var message = new TransferCompletedIntegrationEvent
            {
                TransferId = networkTransfer.Id,
                WalletId = networkTransfer.WalletId
            };

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<TransferCompletedIntegrationEvent>());

            var transfer = await TransferRepository.SingleAsync(x => x.Id == networkTransfer.Id);

            transfer.Status.Should().Be(TransferStatus.Completed);

            await TestHarness.Stop();
        }
    }
}
