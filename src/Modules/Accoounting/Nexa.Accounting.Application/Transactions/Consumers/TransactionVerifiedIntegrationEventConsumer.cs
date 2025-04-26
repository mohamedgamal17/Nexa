using MassTransit;
using Nexa.Accounting.Application.Transactions.Events;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Transactions;
namespace Nexa.Accounting.Application.Transactions.Consumers
{
    public class TransactionVerifiedIntegrationEventConsumer : IConsumer<TransactionVerifiedIntegrationEvent>
    {
        private readonly IAccountingRepository<Transaction> _transactionRepository;

        public TransactionVerifiedIntegrationEventConsumer(IAccountingRepository<Transaction> transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task Consume(ConsumeContext<TransactionVerifiedIntegrationEvent> context)
        {
            var transaction = await _transactionRepository.SingleAsync(x => x.Id == context.Message.TransactionId);

            transaction.Process();

            await _transactionRepository.UpdateAsync(transaction);
        }
    }
}
