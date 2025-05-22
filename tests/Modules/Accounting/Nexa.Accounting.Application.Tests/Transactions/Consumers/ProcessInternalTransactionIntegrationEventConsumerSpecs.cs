using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Nexa.Accounting.Application.Tests.Assertions;
using Nexa.Accounting.Application.Transactions.Events;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Wallets;
using NUnit.Framework.Internal;
namespace Nexa.Accounting.Application.Tests.Transactions.Consumers
{
    [TestFixture]
    [NonParallelizable]
    public class When_process_internal_transaction_integration_event_consumed : TransactionTestFixture
    {
        private ILogger<When_process_internal_transaction_integration_event_consumed> Logger { get; }

        public When_process_internal_transaction_integration_event_consumed()
        {
            Logger = ServiceProvider.GetRequiredService<ILogger<When_process_internal_transaction_integration_event_consumed>>();
        }
        [Test]
        public async Task Should_send_balance_to_reciver_wallet_and_create_two_ledeger_entries_for_both_wallets()
        {
            decimal amountSent = 50;

            decimal senderWallerBalance = 500;

            var fakeSenderWallet = await CreateWalllet(balance : senderWallerBalance);

            var fakeReciverWallet = await CreateWalllet();

            var fakeTransaction
                = await CreateInternalTransaction(fakeSenderWallet.Id, amountSent, fakeReciverWallet.Id, TransactionStatus.Processing);

            var @event = new ProcessInternalTransactionIntgertationEvent(fakeTransaction.Id);

            await TestHarness.Bus.Publish(@event);

      
            Assert.That(await TestHarness.Consumed.Any<ProcessInternalTransactionIntgertationEvent>(), Is.True);

            Logger.LogInformation("Completed Test");
            var rep = ServiceProvider.GetRequiredService<IAccountingRepository<Wallet>>();

            var senderWallet = await rep.SingleAsync(x => x.Id == fakeSenderWallet.Id);

            var reciverWallet = await WalletRepository.SingleAsync(x => x.Id == fakeReciverWallet.Id);

            var transaction = await TransactionRepository.SingleAsync(x => x.Id == fakeTransaction.Id);

            var senderLedgerEntry = await LedgerEntryRepository.SingleOrDefaultAsync(x => x.TransactionId == fakeTransaction.Id && x.WalletId == senderWallet.Id);

            var reciverLedgerEntry = await LedgerEntryRepository.SingleOrDefaultAsync(x => x.TransactionId == fakeTransaction.Id && x.WalletId == reciverWallet.Id);

            senderWallet.Balance.Should().Be(senderWallerBalance - amountSent);

            reciverWallet.Balance.Should().Be(amountSent);

            transaction.Status.Should().Be(TransactionStatus.Completed);

            senderLedgerEntry.Should().NotBeNull();

            senderLedgerEntry!.AssertLedgerEntry(senderWallet.Id, transaction.Id, transaction.Amount, TransactionType.Internal, TransactionDirection.Depit);

            reciverLedgerEntry!.AssertLedgerEntry(reciverWallet.Id, transaction.Id, transaction.Amount, TransactionType.Internal, TransactionDirection.Credit);
        }
    }
}
