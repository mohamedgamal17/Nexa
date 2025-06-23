using MassTransit;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Transfers.Commands.Consumer
{
    public class TransferVerifiedIntegrationEventConsumer : IConsumer<TransferVerifiedIntegrationEvent>
    {
        private readonly ITransferRepository _transferRepository;

        public TransferVerifiedIntegrationEventConsumer(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        public async Task Consume(ConsumeContext<TransferVerifiedIntegrationEvent> context)
        {
            var transfer = await _transferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            transfer.Process();

            await _transferRepository.UpdateAsync(transfer);
        }
    }
}
