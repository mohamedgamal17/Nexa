using MassTransit;
using MediatR;
using Nexa.Transactions.Domain.Events;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Enums;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Transfers.Handlers
{
    public class TransferProcessingEventHandler : INotificationHandler<TransferProcessingEvent>
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ITransferRepository _transferRepository;

        private readonly Dictionary<Type, Func<Transfer, Task>> _transfersHandlers;
        public TransferProcessingEventHandler(IPublishEndpoint publishEndpoint, ITransferRepository transferRepository)
        {
            _publishEndpoint = publishEndpoint;
            _transferRepository = transferRepository;
            _transfersHandlers = new Dictionary<Type, Func<Transfer, Task>>()
            {
                {typeof(NetworkTransfer), HandleNetworkTransfer },
                {typeof(BankTransfer), HandleBankTransfer }
            };

        }


        public async Task Handle(TransferProcessingEvent notification, CancellationToken cancellationToken)
        {
            var transfer = await _transferRepository.SingleAsync(x => x.Id == notification.Id);

            await _transfersHandlers[transfer.GetType()].Invoke(transfer);
        }

        private async Task HandleNetworkTransfer(Transfer transfer)
        {
            var networkTransfer = (NetworkTransfer)transfer;

            var message = new ProcessNetworkTransferIntegrationEvent(networkTransfer.Id, networkTransfer.Number, networkTransfer.WalletId, networkTransfer.ReciverId, networkTransfer.Amount);

            await _publishEndpoint.Publish(message);
        }

        private async Task HandleBankTransfer(Transfer transfer)
        {
            var bankTransfer = (BankTransfer)transfer;

            var message = new ProcessBankTransferIntegrationEvent(transfer.UserId,bankTransfer.Id, bankTransfer.WalletId, bankTransfer.FundingResourceId,
                bankTransfer.Amount, bankTransfer.Direction, bankTransfer.BankTransferType);

            await _publishEndpoint.Publish(message);
        }
    }
}
