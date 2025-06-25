using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Shared.Enums;
using Nexa.Transactions.Shared.Events;

namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class When_process_network_transfer_integration_event_consumed : TransferTestFixture
    {

        [Test]
        public async Task Should_publish_transfer_network__funds_integraiton_event()
        {
            var networkTransfer = await CreateRandomNetworkTransfer(TransferStatus.Processing);

            var message = new ProcessNetworkTransferIntegrationEvent(networkTransfer.Id, networkTransfer.Number, networkTransfer.WalletId, networkTransfer.ReciverId, networkTransfer.Amount);

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<ProcessNetworkTransferIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<TransferNetworkFundsIntegrationEvent>());
        }
    }
}
