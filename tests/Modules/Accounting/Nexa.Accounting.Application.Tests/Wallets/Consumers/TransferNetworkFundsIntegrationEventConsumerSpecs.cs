using FluentAssertions;
using Nexa.Accounting.Application.Tests.Assertions;
using Nexa.Accounting.Shared.Events;

namespace Nexa.Accounting.Application.Tests.Wallets.Consumers
{
    [TestFixture]
    public class When_transfer_network_funds_event_consumed : WalletTestFixture
    {

        [Test]
        public async Task Should_transfer_funds_from_sender_wallet_to_reciver_wallet_and_create_ledger_entries()
        {
            var fakeSenderWallet = await CreateWalletWithReservedBalanceAsync(balance: 1000, reservedBalance: 300);
            var fakeReciverWallet = await CreateWalletAsync();

            var message = new TransferNetworkFundsIntegrationEvent(Guid.NewGuid().ToString(), fakeSenderWallet.Id, fakeReciverWallet.Id, 300);

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<TransferNetworkFundsIntegrationEvent>());
            Assert.That(await TestHarness.Published.Any<NetworkFundsTransferredIntegrationEvent>());

            var senderWallet = await WalletRepository.SingleAsync(x => x.Id == fakeSenderWallet.Id);
            var reciverWallet = await  WalletRepository.SingleAsync(x => x.Id == fakeReciverWallet.Id);
            var senderLedgerEntry = await LedgerEntryRepository.SingleOrDefaultAsync(x => x.WalletId == senderWallet.Id && x.TransactionId == message.TransferId);
            var reciverLedgerEntry = await LedgerEntryRepository.SingleOrDefaultAsync(x => x.WalletId == reciverWallet.Id && x.TransactionId == message.TransferId);

            senderWallet.ReservedBalance.Should().Be(0);
            senderWallet.Balance.Should().Be(700);
            reciverWallet.Balance.Should().Be(300);

            senderLedgerEntry.Should().NotBeNull();
            senderLedgerEntry!.AssertLedgerEntry(
                    senderWallet.Id,
                    message.TransferId,
                    message.Amount,
                    Domain.Enums.TransferType.Network,
                    Domain.Enums.TransferDirection.Depit
                );

            reciverLedgerEntry.Should().NotBeNull();
            reciverLedgerEntry!.AssertLedgerEntry(
                    reciverWallet.Id,
                    message.TransferId,
                    message.Amount,
                    Domain.Enums.TransferType.Network,
                    Domain.Enums.TransferDirection.Credit
                );
        }

    }
}
