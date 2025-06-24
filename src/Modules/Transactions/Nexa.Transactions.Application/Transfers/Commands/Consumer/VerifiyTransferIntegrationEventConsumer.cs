using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Enums;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Transfers.Commands.Consumer
{
    public class VerifiyTransferIntegrationEventConsumer : IConsumer<VerifiyTransferIntegerationEvent>
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
                { typeof(AchTransfer), ProcessAchTransfer },
                { typeof(WireTransfer), ProcessWireTransfer },
            };
        }

        public async Task Consume(ConsumeContext<VerifiyTransferIntegerationEvent> context)
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

        private async Task ProcessAchTransfer(Transfer transfer)
        {
            var achTransfer = (AchTransfer)transfer;

            if(achTransfer.Direction == TransferDirection.Depit)
            {
                var message = new ReserveWalletBalanceIntegrationEvent
                {
                    TransferId = achTransfer.Id,
                    WalletId = achTransfer.WalletId,
                    Amount = achTransfer.Amount
                };

                await _publishEndpoint.Publish(message);
            }
            else
            {
                var message = new TransferVerifiedIntegrationEvent(achTransfer.Id, achTransfer.Number,
                    achTransfer.WalletId, achTransfer.Amount, achTransfer.Type);

                await _publishEndpoint.Publish(message);
            }
        }


        private async Task ProcessWireTransfer(Transfer transfer)
        {
            var wireTransfer = (WireTransfer)transfer;

            if (wireTransfer.Direction == TransferDirection.Depit)
            {
                var message = new ReserveWalletBalanceIntegrationEvent
                {
                    TransferId = wireTransfer.Id,
                    WalletId = wireTransfer.WalletId,
                    Amount = wireTransfer.Amount
                };

                await _publishEndpoint.Publish(message);
            }
            else
            {
                var message = new TransferVerifiedIntegrationEvent(wireTransfer.Id, wireTransfer.Number,
                   wireTransfer.WalletId, wireTransfer.Amount, wireTransfer.Type);

                await _publishEndpoint.Publish(message);
            }
        }
    }
}
