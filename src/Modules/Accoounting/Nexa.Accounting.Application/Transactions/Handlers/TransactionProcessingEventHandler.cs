using MassTransit;
using MediatR;
using Nexa.Accounting.Application.Transactions.Events;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Transactions;
using Nexa.Accounting.Domain.Transactions.Events;

namespace Nexa.Accounting.Application.Transactions.Handlers
{
    public class TransactionProcessingEventHandler : INotificationHandler<TransactionProcessingEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public TransactionProcessingEventHandler(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Handle(TransactionProcessingEvent notification, CancellationToken cancellationToken)
        {
            if(notification.Type == TransactionType.External)          
            {
                var @event = new ProcessExternalTransactionIntgerationEvent(notification.Id);

                await _publishEndpoint.Publish(@event);
            }
            else
            {
                var @event = new ProcessInternalTransactionIntgertationEvent(notification.Id);

                await _publishEndpoint.Publish(@event);
            }
        }
    }
}
