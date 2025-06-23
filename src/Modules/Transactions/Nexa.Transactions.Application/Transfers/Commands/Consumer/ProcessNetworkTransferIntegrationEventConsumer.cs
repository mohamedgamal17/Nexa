using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Transfers.Commands.Consumer
{
    public class ProcessNetworkTransferIntegrationEventConsumer : IConsumer<ProcessNetworkTransferIntegrationEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public ProcessNetworkTransferIntegrationEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ProcessNetworkTransferIntegrationEvent> context)
        {
            var message = new TransferNetworkFundsIntegrationEvent(context.Message.TransferId, context.Message.WalletId, context.Message.ReciverId,context.Message.Amount);

            await _publishEndpoint.Publish(message);
        }
    }
}
