using MassTransit;
using MediatR;
using Nexa.Accounting.Application.Transactions.Events;
using Nexa.Accounting.Domain.Transactions.Events;

namespace Nexa.Accounting.Application.Transactions.Handlers
{
    public class TransactionPendingEventHandler : INotificationHandler<TransactionPendingEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public TransactionPendingEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(TransactionPendingEvent notification, CancellationToken cancellationToken)
        {
            var @event = new RequestTransactionVerificationIntgertationEvent(notification.Id, notification.Type);

            await _publishEndpoint.Publish(@event);
        }
    }
}
