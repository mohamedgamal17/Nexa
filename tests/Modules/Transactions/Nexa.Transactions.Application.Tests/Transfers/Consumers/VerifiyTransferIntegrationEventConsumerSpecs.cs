using MassTransit.Testing;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Shared.Enums;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class When_verifiy_transfer_integration_event_consumed : TransferTestFixture
    {
        [Test]
        public async Task Should_publish_reserve_wallet_balance_message_when_current_transfer_is_network_transfer()
        {

            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var senderWallet = await CreateWalletAsync(userId, 1000);

            var reciverWallet = await CreateWalletAsync(userId, 1000);

            var networkTransfer = await CreateNetworkTransferAsync(userId, senderWallet.Id, reciverWallet.Id, 500);

            var message = new VerifiyTransferIntegrationEvent(networkTransfer.Id, networkTransfer.Number, networkTransfer.WalletId, networkTransfer.Amount, TransferType.Network);

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<VerifiyTransferIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<ReserveWalletBalanceIntegrationEvent>());
        }

        [Test]
        public async Task Should_publish_reserve_wallet_balance_message_when_current_transfer_is_bank_transfer_and_direction_is_depit()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeFundingResoruce = await CreateFundingResourceAsync(userId);

            var fakeWallet = await CreateWalletAsync(userId, 500);

            var fakeTransfer = await CreateBankTransferAsync(userId, fakeWallet.Id, fakeFundingResoruce.Id, 100, TransferDirection.Depit);

            await TestHarness.Start();

            var message = new VerifiyTransferIntegrationEvent(fakeTransfer.Id, fakeTransfer.Number, fakeTransfer.WalletId, fakeTransfer.Amount, fakeTransfer.Type);

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<VerifiyTransferIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<ReserveWalletBalanceIntegrationEvent>());

            await TestHarness.Stop();

        }

        [Test]
        public async Task Should_publish_transfer_verified_message_when_current_transfer_is_bank_transfer_and_direction_is_credit()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeFundingResoruce = await CreateFundingResourceAsync(userId);

            var fakeWallet = await CreateWalletAsync(userId, 500);

            var fakeTransfer = await CreateBankTransferAsync(userId, fakeWallet.Id, fakeFundingResoruce.Id, 100, TransferDirection.Credit);

            await TestHarness.Start();

            var message = new VerifiyTransferIntegrationEvent(fakeTransfer.Id, fakeTransfer.Number, fakeTransfer.WalletId, fakeTransfer.Amount, fakeTransfer.Type);

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<VerifiyTransferIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<TransferVerifiedIntegrationEvent>());

            await TestHarness.Stop();

        }


    }
}
