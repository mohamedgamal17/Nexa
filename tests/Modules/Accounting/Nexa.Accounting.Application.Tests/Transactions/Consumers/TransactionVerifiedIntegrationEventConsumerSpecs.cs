using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Transactions.Events;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Transactions;

namespace Nexa.Accounting.Application.Tests.Transactions.Consumers
{
    [TestFixture]
    [NonParallelizable]
    public class When_transaction_verified_integration_event_consumed : TransactionTestFixture
    {
        protected IAccountingRepository<Transaction> TransactionRepository { get; }

        public When_transaction_verified_integration_event_consumed()
        {
            TransactionRepository = ServiceProvider.GetRequiredService<IAccountingRepository<Transaction>>();
        }

        [Test]
        public async Task Should_update_transaction_state_and_sent_process_internal_tranasction_integration_event()
        {
            var senderWallet = await CreateWalllet(balance: 500);

            var reciverWallet = await CreateWalllet();

            var fakeTransaction = await CreateInternalTransaction(senderWallet.Id, 50, reciverWallet.Id, TransactionStatus.Pending);

            var @event = new TransactionVerifiedIntegrationEvent(fakeTransaction.Id, TransactionType.Internal);

            await TestHarness.Bus.Publish(@event);

            Assert.That(await TestHarness.Consumed.Any<TransactionVerifiedIntegrationEvent>());

            Assert.That(await TestHarness.Published.Any<ProcessInternalTransactionIntgertationEvent>());

            var tranasction = await TransactionRepository.SingleAsync(x => x.Id == fakeTransaction.Id);

            tranasction.Status.Should().Be(TransactionStatus.Processing);
        }

    }
}
