using MassTransit;
using MediatR;
using Nexa.Accounting.Application.Wallets.Commands.CreateWallet;
using Nexa.CustomerManagement.Shared.Events;
namespace Nexa.Accounting.Application.Wallets.Consumers
{
    public class CustomerCreatedIntegrationEventConsumer : IConsumer<CustomerCreatedIntegrationEvent>
    {
        private readonly IMediator _mediator;

        public CustomerCreatedIntegrationEventConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<CustomerCreatedIntegrationEvent> context)
        {
            var command = new CreateWalletCommand
            {
                UserId = context.Message.UserId,
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
