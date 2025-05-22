using Nexa.Accounting.Application.Transactions.Events;
using Nexa.Accounting.Domain.Enums;
namespace Nexa.Accounting.Application.Tests.Transactions.Consumers
{
    [TestFixture]
    [NonParallelizable]
    public class When_request_transaction_verifiecation_intgertation_consumed : TransactionTestFixture
    {
        [Test]
        public async Task Should_verifiy_internal_transaction_and_publish_transaction_verified_event()
        {
            var senderWallet = await CreateWalllet(balance: 500);

            var reciverWallet = await CreateWalllet();

            var transaction = await CreateInternalTransaction(senderWallet.Id, 50, reciverWallet.Id, TransactionStatus.Pending);

            var @event = new RequestTransactionVerificationIntgertationEvent(transaction.Id, TransactionType.Internal);

            await TestHarness.Bus.Publish(@event);

            Assert.That(await TestHarness.Consumed.Any<RequestTransactionVerificationIntgertationEvent>());

            Assert.That(await TestHarness.Published.Any<TransactionVerifiedIntegrationEvent>());
        }

        [Test]
        public async Task Should_verifiy_external_transaction_and_publish_transaction_verified_event()
        {
            var wallet = await CreateWalllet();

            var transaction = await CreateExternalTransaction(wallet.Id, Guid.NewGuid().ToString(), 50, TransactionDirection.Credit, TransactionStatus.Pending);

            var @event = new RequestTransactionVerificationIntgertationEvent(transaction.Id, TransactionType.External);

            await TestHarness.Bus.Publish(@event);

            Assert.That(await TestHarness.Consumed.Any<RequestTransactionVerificationIntgertationEvent>());

            Assert.That(await TestHarness.Published.Any<TransactionVerifiedIntegrationEvent>());
        }    
    }
}
