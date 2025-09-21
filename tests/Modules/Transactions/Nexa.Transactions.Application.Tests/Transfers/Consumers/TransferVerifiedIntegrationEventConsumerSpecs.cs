using FluentAssertions;
using MassTransit.Testing;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Shared.Enums;
using Nexa.Transactions.Shared.Events;

namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class When_transfer_verified_integration_event_consumed : TransferTestFixture
    {
        [Test]
        public async Task Should_update_transfer_state_from_pending_to_processing()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var senderWallet = await CreateWalletAsync(userId, 1000);

            var reciverWallet = await CreateWalletAsync(userId, 1000);

            var networkTransfer = await CreateNetworkTransferAsync(userId, senderWallet.Id, reciverWallet.Id, 500);

            await TestHarness.Start();

            var message = new TransferVerifiedIntegrationEvent
            {
                TransferId = networkTransfer.Id,
                WalletId = networkTransfer.WalletId,
                Amount = networkTransfer.Amount
            };

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<TransferVerifiedIntegrationEvent>());

            var transfer = await TransferRepository.SingleAsync(x => x.Id == networkTransfer.Id);

            transfer.Status.Should().Be(TransferStatus.Processing);

            await TestHarness.Stop();


        }
    }
}
