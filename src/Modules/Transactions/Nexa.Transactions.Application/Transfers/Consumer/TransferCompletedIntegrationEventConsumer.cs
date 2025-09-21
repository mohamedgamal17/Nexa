using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Domain.Transfers;
namespace Nexa.Transactions.Application.Transfers.Consumer
{
    public class TransferCompletedIntegrationEventConsumer : IConsumer<TransferCompletedIntegrationEvent>
    {
        private readonly ITransferRepository _transferRepository;

        public TransferCompletedIntegrationEventConsumer(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        public async Task Consume(ConsumeContext<TransferCompletedIntegrationEvent> context)
        {
            var transfer = await _transferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            transfer.Complete();

            await _transferRepository.UpdateAsync(transfer);
        }
    }
}
