using MassTransit;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Enums;
using Nexa.Accounting.Shared.Events;

namespace Nexa.Accounting.Application.Wallets.Consumers
{
    public class ReciveBalanceIntegrationEventConsumer : IConsumer<ReciveBalanceIntegrationEvent>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IAccountingRepository<LedgerEntry> _ledgerEntryRepository;

        public ReciveBalanceIntegrationEventConsumer(IWalletRepository walletRepository, IPublishEndpoint publishEndpoint, IAccountingRepository<LedgerEntry> ledgerEntryRepository)
        {
            _walletRepository = walletRepository;
            _publishEndpoint = publishEndpoint;
            _ledgerEntryRepository = ledgerEntryRepository;
        }

        public async Task Consume(ConsumeContext<ReciveBalanceIntegrationEvent> context)
        {
            var wallet = await _walletRepository.SingleAsync(x => x.Id == context.Message.WalletId);

            wallet.Credit(context.Message.Amount);

            var ledgerEntry = new LedgerEntry(wallet.Id, context.Message.Amount, TransferType.Bank,
                 TransferDirection.Credit, context.Message.TransferId, context.Message.CompletedAt);

            await _walletRepository.UpdateAsync(wallet);

            await _ledgerEntryRepository.InsertAsync(ledgerEntry);


            var integrationEvent = new TransferCompletedIntegrationEvent
            {
                TransferId = context.Message.TransferId,
                WalletId = context.Message.WalletId
            };

            await _publishEndpoint.Publish(integrationEvent);
        }
    }

}
