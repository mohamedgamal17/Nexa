using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Events;

namespace Nexa.Transactions.Application.Transfers.Consumer
{
    public class WalletBalanceReservedIntegrationEventConsumer : IConsumer<WalletBalanceReservedIntegrationEvent>
    {
        private readonly ITransferRepository _transferRepository;

        private readonly IPublishEndpoint _publishEndpoint;
        public WalletBalanceReservedIntegrationEventConsumer(ITransferRepository transferRepository, IPublishEndpoint publishEndpoint)
        {
            _transferRepository = transferRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<WalletBalanceReservedIntegrationEvent> context)
        {
            var transfer = await _transferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            var message = new TransferVerifiedIntegrationEvent(transfer.Id, transfer.Number, transfer.WalletId, transfer.Amount, transfer.Type);

            await _publishEndpoint.Publish(message);
        }
    }
}
