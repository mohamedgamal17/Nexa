using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Events;

namespace Nexa.Transactions.Application.Transfers.Consumer
{
    public class ExternalTransferCompletedEventConsumer : IConsumer<ExternalTransferCompletedIntegrationEvent>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ExternalTransferCompletedEventConsumer(ITransferRepository transferRepository, IPublishEndpoint publishEndpoint)
        {
            _transferRepository = transferRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ExternalTransferCompletedIntegrationEvent> context)
        {
            var transfer = await _transferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            if(transfer is BankTransfer bankTransfer)
            {
                if(bankTransfer.Direction == Shared.Enums.TransferDirection.Credit)
                {
                    var @event = new ReciveBalanceIntegrationEvent
                    {
                        WalletId = bankTransfer.WalletId,
                        TransferId = bankTransfer.Id,
                        TransferNumber = bankTransfer.Number,
                        FundingResourceId = bankTransfer.FundingResourceId,
                        Amount = bankTransfer.Amount,
                        CompletedAt = DateTime.UtcNow
                    };

                    await _publishEndpoint.Publish(@event);
                }
            }
        }
    }
}
