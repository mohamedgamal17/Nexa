using MassTransit;
using MediatR;
using Nexa.Transactions.Domain.Events;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Transfers.Handlers
{
    public class TransferPendingEventEventHandler : INotificationHandler<TransferPendingEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public TransferPendingEventEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(TransferPendingEvent notification, CancellationToken cancellationToken)
        {
            var @event = new VerifiyTransferIntegrationEvent(notification.Id, notification.Number, notification.WalletId, notification.Amount, notification.Type);

            await _publishEndpoint.Publish(@event);
        }
    }
}
