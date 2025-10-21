using FluentAssertions;
using MassTransit.Testing;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Shared.Enums;
using Nexa.Transactions.Shared.Events;

namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class When_process_network_transfer_integration_event_consumed : TransferTestFixture
    {

        [Test]
        public async Task Should_pubish_transfer_network_funds_integraiton_event()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var senderWallet = await CreateWalletAsync(userId, 1000);

            var reciverWallet = await CreateWalletAsync(userId, 1000);

            var fakeNetworkTransfer = await CreateProcessNetworkTransferAsync(userId, senderWallet.Id, reciverWallet.Id, 500);

            await TestHarness.Start();

            var message = new ProcessNetworkTransferIntegrationEvent(fakeNetworkTransfer.Id, fakeNetworkTransfer.Number, fakeNetworkTransfer.WalletId, fakeNetworkTransfer.ReciverId, fakeNetworkTransfer.Amount);

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<ProcessNetworkTransferIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<TransferNetworkFundsIntegrationEvent>());

            await TestHarness.Stop();

        }
    }
}
