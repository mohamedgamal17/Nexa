using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Domain;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Transfers.Consumer
{
    public class ExternalTransferCompletedEventConsumer : IConsumer<ExternalTransferCompletedIntegrationEvent>
    {
        private readonly ITransactionRepository<BankTransfer> _bankAccountTransferRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ExternalTransferCompletedEventConsumer(ITransactionRepository<BankTransfer> bankAccountTransferRepository, IPublishEndpoint publishEndpoint)
        {
            _bankAccountTransferRepository = bankAccountTransferRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ExternalTransferCompletedIntegrationEvent> context)
        {
            var bankTransfer = await _bankAccountTransferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            if (bankTransfer.Direction == Shared.Enums.TransferDirection.Credit)
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
            else
            {

                var @event = new TransferFundsIntegrationEvent
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
