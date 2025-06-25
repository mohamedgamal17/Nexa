using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Domain.Transfers;

namespace Nexa.Transactions.Application.Transfers.Consumer
{
    public class WalletBalanceReservationFailedIntegrationEventConsumer : IConsumer<WalletBalanceReservationFailedIntegrationEvent>
    {
        private readonly ITransferRepository _transferRepository;

        public WalletBalanceReservationFailedIntegrationEventConsumer(ITransferRepository transferRepository)
        {
            _transferRepository = transferRepository;
        }

        public async Task Consume(ConsumeContext<WalletBalanceReservationFailedIntegrationEvent> context)
        {
            var transfer = await _transferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            transfer.Cancel();

            await _transferRepository.UpdateAsync(transfer);
        }
    }
}
