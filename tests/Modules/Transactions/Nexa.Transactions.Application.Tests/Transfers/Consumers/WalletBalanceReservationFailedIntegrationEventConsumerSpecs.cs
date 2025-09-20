using FluentAssertions;
using MassTransit.Testing;
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
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var senderWallet = await CreateWalletAsync(userId, 1000);

            var reciverWallet = await CreateWalletAsync(userId, 1000);

            var networkTransfer = await CreateNetworkTransferAsync(userId, senderWallet.Id, reciverWallet.Id, 500);

            await TestHarness.Start();


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

            await TestHarness.Stop();

        }
    }
}
