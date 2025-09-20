using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Domain;
using Nexa.Transactions.Domain.Transfers;
namespace Nexa.Transactions.Application.Transfers.Consumer
{

    public class ReciveBalanceCompletedIntegrationEventConsumer : IConsumer<ReciveBalanceCompletedIntegrationEvent>
    {
        private readonly ITransactionRepository<BankTransfer> _transferRepository;

        public ReciveBalanceCompletedIntegrationEventConsumer(ITransactionRepository<BankTransfer> transferRepository)
        {
            _transferRepository = transferRepository;
        }

        public async Task Consume(ConsumeContext<ReciveBalanceCompletedIntegrationEvent> context)
        {
            var transfer = await _transferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            transfer.Complete();

            await _transferRepository.UpdateAsync(transfer);
        }
    }
}
