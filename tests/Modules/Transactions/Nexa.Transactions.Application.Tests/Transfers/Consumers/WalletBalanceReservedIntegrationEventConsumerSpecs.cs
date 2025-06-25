using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Shared.Events;

namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class When_wallet_balance_reserved_integration_event_consumed : TransferTestFixture
    {
        [Test]
        public async Task Should_publish_transaction_verified_integration_event()
        {
            var transfer = await CreateRandomNetworkTransfer();

            var message = new WalletBalanceReservedIntegrationEvent()
            {
                TransferId = transfer.Id,
                WalletId = transfer.WalletId,
                Amount = transfer.Amount
            };

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<WalletBalanceReservedIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<TransferVerifiedIntegrationEvent>());
        }
    }
}
