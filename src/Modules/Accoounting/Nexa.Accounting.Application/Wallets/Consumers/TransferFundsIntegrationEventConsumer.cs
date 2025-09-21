using MassTransit;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Events;

namespace Nexa.Accounting.Application.Wallets.Consumers
{
    public class TransferFundsIntegrationEventConsumer : IConsumer<TransferFundsIntegrationEvent>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IAccountingRepository<LedgerEntry> _ledgerEntryRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        public TransferFundsIntegrationEventConsumer(IWalletRepository walletRepository, IAccountingRepository<LedgerEntry> ledgerEntryRepository, IPublishEndpoint publishEndpoint)
        {
            _walletRepository = walletRepository;
            _ledgerEntryRepository = ledgerEntryRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<TransferFundsIntegrationEvent> context)
        {
            var wallet = await _walletRepository.SingleAsync(x => x.Id == context.Message.WalletId);

            wallet.Depit(context.Message.Amount);

            var ledgerEntry = new LedgerEntry(
                    wallet.Id,
                    context.Message.Amount,
                    TransferType.Bank,
                    TransferDirection.Depit,
                    context.Message.TransferId,
                    DateTime.UtcNow
                );

            await _walletRepository.UpdateAsync(wallet);

            await _ledgerEntryRepository.InsertAsync(ledgerEntry);

            var @event = new TransferCompletedIntegrationEvent
            {
                WalletId = wallet.Id,
                TransferId = context.Message.TransferId
            };

            await _publishEndpoint.Publish(@event);    
        }
    }
}
