using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Enums;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Transfers.Commands.Consumer
{
    public class VerifiyTransferIntegrationEventConsumer : IConsumer<VerifiyTransferIntegrationEvent>
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly Dictionary<Type, Func<Transfer,Task>> _transferHandlers;
        public VerifiyTransferIntegrationEventConsumer(ITransferRepository transferRepository, IPublishEndpoint publishEndpoint)
        {
            _transferRepository = transferRepository;
            _publishEndpoint = publishEndpoint;
            _transferHandlers = new Dictionary<Type, Func<Transfer, Task>>
            {
                { typeof(NetworkTransfer), ProcessNetworkTransfer },
                { typeof(BankTransfer), ProcessBankTransfer },
            };
        }

        public async Task Consume(ConsumeContext<VerifiyTransferIntegrationEvent> context)
        {
            var transfer = await _transferRepository.SingleAsync(x => x.Id == context.Message.TransferId);

            await _transferHandlers[transfer.GetType()].Invoke(transfer);
        }

        private async Task ProcessNetworkTransfer(Transfer transfer)
        {
            var message = new ReserveWalletBalanceIntegrationEvent
            {
                TransferId = transfer.Id,
                WalletId = transfer.WalletId,
                Amount = transfer.Amount
            };

            await _publishEndpoint.Publish(message);
        }

        private async Task ProcessBankTransfer(Transfer transfer)
        {
            var bankTransfer = (BankTransfer)transfer;

            if(bankTransfer.Direction == TransferDirection.Depit)
            {
                var message = new ReserveWalletBalanceIntegrationEvent
                {
                    TransferId = bankTransfer.Id,
                    WalletId = bankTransfer.WalletId,
                    Amount = bankTransfer.Amount
                };

                await _publishEndpoint.Publish(message);
            }
            else
            {
                var message = new TransferVerifiedIntegrationEvent(bankTransfer.Id, bankTransfer.Number,
                    bankTransfer.WalletId, bankTransfer.Amount, bankTransfer.Type);

                await _publishEndpoint.Publish(message);
            }
        }
    }
}
