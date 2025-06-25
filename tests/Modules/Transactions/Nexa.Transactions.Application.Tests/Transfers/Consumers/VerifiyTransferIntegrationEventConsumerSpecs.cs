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
            var transfer = await CreateRandomNetworkTransfer();

            var message = new VerifiyTransferIntegrationEvent(transfer.Id, transfer.Number, transfer.WalletId, transfer.Amount, TransferType.Network);

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<VerifiyTransferIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<ReserveWalletBalanceIntegrationEvent>());
        }

        [Test]
        public async Task Should_publish_reserve_wallet_balance_message_when_current_transfer_is_bank_transfer_and_direction_is_depit()
        {
            var transfer = await CreateRandomBankTransferAsync(BankTransferType.Ach,TransferDirection.Depit);

            var message = new VerifiyTransferIntegrationEvent(transfer.Id, transfer.Number, transfer.WalletId, transfer.Amount, transfer.Type);


            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<VerifiyTransferIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<ReserveWalletBalanceIntegrationEvent>());
        }

        [Test]
        public async Task Should_publish_transfer_verified_message_when_current_transfer_is_bank_transfer_and_direction_is_credit()
        {
            var transfer = await CreateRandomBankTransferAsync(BankTransferType.Ach, TransferDirection.Credit);

            var message = new VerifiyTransferIntegrationEvent(transfer.Id, transfer.Number, transfer.WalletId, transfer.Amount, transfer.Type);

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<VerifiyTransferIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<TransferVerifiedIntegrationEvent>());
        }


    }
}
