using MassTransit;
using MediatR;
using Nexa.Accounting.Application.Wallets.Commands.FreezeWallet;
using Nexa.CustomerManagement.Shared.Events;
namespace Nexa.Accounting.Application.Wallets.Consumers
{
    public class CustomerRejectedIntegrationEventConsumer : IConsumer<CustomerRejectedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public CustomerRejectedIntegrationEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<CustomerRejectedIntegrationEvent> context)
        {
            var command = new FreezeWalletCommand
            {
                CustomerId = context.Message.CustomerId
            };

            var result = await _mediator.Send(command);

            if (result.IsFailure)
            {
                throw result.Exception!;
            }
        }
    }
}
