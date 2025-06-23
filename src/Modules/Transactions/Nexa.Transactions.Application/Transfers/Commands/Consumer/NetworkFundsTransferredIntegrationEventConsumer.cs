using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Domain.Transfers;
namespace Nexa.Transactions.Application.Transfers.Commands.Consumer
{
    public class NetworkFundsTransferredIntegrationEventConsumer : IConsumer<NetworkFundsTransferredIntegrationEvent>
    {
        private readonly ITransferRepository _transferRepository;

        public NetworkFundsTransferredIntegrationEventConsumer(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        public async Task Consume(ConsumeContext<NetworkFundsTransferredIntegrationEvent> context)
        {
            var transfer = await _transferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            transfer.Complete();

            await _transferRepository.UpdateAsync(transfer);
        }
    }
}
