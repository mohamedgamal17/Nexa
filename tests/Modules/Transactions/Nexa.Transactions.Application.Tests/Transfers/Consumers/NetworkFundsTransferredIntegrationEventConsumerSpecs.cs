using FluentAssertions;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class When_network_funds_transferred_integration_event_consumed : TransferTestFixture
    {
        [Test]
        public async Task Should_update_transfer_status_to_complete()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var senderWallet = await CreateWalletAsync(userId, 1000);

            var reciverWallet = await CreateWalletAsync(userId, 1000);

            var networkTransfer = await CreateProcessNetworkTransferAsync(userId, senderWallet.Id, reciverWallet.Id, 500);

            var message = new NetworkFundsTransferredIntegrationEvent(networkTransfer.Id, networkTransfer.WalletId, networkTransfer.ReciverId, networkTransfer.Amount);

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<NetworkFundsTransferredIntegrationEvent>());

            var transfer = await TransferRepository.SingleAsync(x => x.Id == networkTransfer.Id);

            transfer.Status.Should().Be(TransferStatus.Completed);
        }
    }
}
