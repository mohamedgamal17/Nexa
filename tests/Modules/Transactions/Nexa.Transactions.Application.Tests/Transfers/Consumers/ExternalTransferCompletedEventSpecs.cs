using MassTransit.Testing;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class  When_external_transfer_completed_integration_event_consumer : TransferTestFixture
    {
        [Test]
        public async Task Should_publish_recive_balance_integration_event_when_transfer_is_deposit_bank_transfer()
        {

            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeWallet = await CreateWalletAsync(userId, 100);

            var fakeFundingResource = await CreateFundingResourceAsync(userId);

            var fakeTransfer = await CreateProcessBankTransferAsync(userId, fakeWallet.Id, fakeFundingResource.Id, 50, Shared.Enums.TransferDirection.Credit);

            await TestHarness.Start();

            var @event = new ExternalTransferCompletedIntegrationEvent
            {
                TransferId = fakeTransfer.Id,
                ExternalTransferId = fakeTransfer.ExternalTransferId!
            };

            await TestHarness.Bus.Publish(@event);

            Assert.That(await TestHarness.Consumed.Any<ExternalTransferCompletedIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<ReciveBalanceIntegrationEvent>());

            await TestHarness.Stop();

        }

        [Test]
        public async Task Should_publish_transfer_funds_integration_event_when_transfer_withdraw_bank_transfer()
        {

            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeWallet = await CreateWalletAsync(userId, 100);

            var fakeFundingResource = await CreateFundingResourceAsync(userId);

            var fakeTransfer = await CreateProcessBankTransferAsync(userId, fakeWallet.Id, fakeFundingResource.Id, 50, Shared.Enums.TransferDirection.Depit);

            await TestHarness.Start();

            var @event = new ExternalTransferCompletedIntegrationEvent
            {
                TransferId = fakeTransfer.Id,
                ExternalTransferId = fakeTransfer.ExternalTransferId!
            };

            await TestHarness.Bus.Publish(@event);

            Assert.That(await TestHarness.Consumed.Any<ExternalTransferCompletedIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<TransferFundsIntegrationEvent>());

            await TestHarness.Stop();
        }
    }
}
