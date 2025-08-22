using MassTransit;
using MediatR;
using Nexa.Accounting.Application.Wallets.Commands.ActivateWallet;
using Nexa.CustomerManagement.Shared.Events;

namespace Nexa.Accounting.Application.Wallets.Consumers
{
    public class CustomerAcceptedIntegrationEventConsumer : IConsumer<CustomerAcceptedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public CustomerAcceptedIntegrationEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<CustomerAcceptedIntegrationEvent> context)
        {
            var command = new ActivateWalletCommand
            {
                FintechCustomerId = context.Message.FintechCustomerId,
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
