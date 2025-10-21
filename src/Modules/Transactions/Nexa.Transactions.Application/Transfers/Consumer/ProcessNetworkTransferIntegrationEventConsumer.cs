using MassTransit;
using Nexa.Accounting.Shared.Events;
using Nexa.Transactions.Domain;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Events;
namespace Nexa.Transactions.Application.Transfers.Consumer
{
    public class ProcessNetworkTransferIntegrationEventConsumer : IConsumer<ProcessNetworkTransferIntegrationEvent>
    {

        private readonly ITransactionRepository<NetworkTransfer> _networkTransferRepository;
        private readonly IPublishEndpoint _publishEndpoint;

        public ProcessNetworkTransferIntegrationEventConsumer(ITransactionRepository<NetworkTransfer> networkTransferRepository, IPublishEndpoint publishEndpoint)
        {
            _networkTransferRepository = networkTransferRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<ProcessNetworkTransferIntegrationEvent> context)
        {
            var networkTransfer = await _networkTransferRepository.SingleAsync(x => x.Id == context.Message.TransferId);


            var @event = new TransferNetworkFundsIntegrationEvent
                    (
                        networkTransfer.UserId,
                        networkTransfer.Id,
                        networkTransfer.WalletId,
                        networkTransfer.ReciverId,
                        networkTransfer.Amount
                    );

      
            await _publishEndpoint.Publish(@event);           
        }
    }
}
