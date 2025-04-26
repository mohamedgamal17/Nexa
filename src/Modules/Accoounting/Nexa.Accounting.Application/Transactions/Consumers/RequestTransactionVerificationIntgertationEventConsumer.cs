using MassTransit;
using Nexa.Accounting.Application.Transactions.Events;
namespace Nexa.Accounting.Application.Transactions.Consumers
{
    public class RequestTransactionVerificationIntgertationEventConsumer : IConsumer<RequestTransactionVerificationIntgertationEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public RequestTransactionVerificationIntgertationEventConsumer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<RequestTransactionVerificationIntgertationEvent> context)
        {
            var @event = new TransactionVerifiedIntegrationEvent(context.Message.TransactionId, context.Message.TransactionType);

            await _publishEndpoint.Publish(@event);
        }
    }
}
