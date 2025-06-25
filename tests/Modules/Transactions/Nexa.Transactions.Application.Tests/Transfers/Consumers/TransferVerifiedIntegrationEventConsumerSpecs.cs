using FluentAssertions;
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
            var networkTransfer = await CreateRandomNetworkTransfer();

            var message = new TransferVerifiedIntegrationEvent(networkTransfer.Id, networkTransfer.Number, networkTransfer.WalletId, networkTransfer.Amount, networkTransfer.Type);

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<TransferVerifiedIntegrationEvent>());

            var transfer = await TransferRepository.SingleAsync(x => x.Id == networkTransfer.Id);

            transfer.Status.Should().Be(TransferStatus.Processing);

        }
    }
}
