using MassTransit.Testing;
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
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var senderWallet = await CreateWalletAsync(userId, 1000);

            var reciverWallet = await CreateWalletAsync(userId, 1000);

            var networkTransfer = await CreateNetworkTransferAsync(userId, senderWallet.Id, reciverWallet.Id, 500);

            await TestHarness.Start();

            var message = new WalletBalanceReservedIntegrationEvent()
            {
                TransferId = networkTransfer.Id,
                WalletId = networkTransfer.WalletId,
                Amount = networkTransfer.Amount
            };

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<WalletBalanceReservedIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<TransferVerifiedIntegrationEvent>());

            await TestHarness.Stop();

        }
    }
}
