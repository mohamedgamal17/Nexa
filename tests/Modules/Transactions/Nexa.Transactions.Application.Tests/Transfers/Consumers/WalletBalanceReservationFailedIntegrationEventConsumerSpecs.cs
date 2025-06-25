using FluentAssertions;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class When_wallet_balance_reservation_failed_event_consumed : TransferTestFixture
    {
        [Test]
        public async Task Should_cancel_transfer()
        {
            var networkTransfer = await CreateRandomNetworkTransfer();

            var message = new WalletBalanceReservationFailedIntegrationEvent()
            {
                TransferId = networkTransfer.Id,
                WalletId = networkTransfer.WalletId,
                Amount = networkTransfer.Amount
            };

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<WalletBalanceReservationFailedIntegrationEvent>());

            var transfer = await TransferRepository.SingleAsync(x => x.Id == networkTransfer.Id);

            transfer.Status.Should().Be(TransferStatus.Faild);
        }
    }
}
