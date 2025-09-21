using MassTransit;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Events;
namespace Nexa.Accounting.Application.Wallets.Consumers
{
    public class TransferNetworkFundsIntegrationEventConsumer : IConsumer<TransferNetworkFundsIntegrationEvent>
    {
        private readonly IWalletRepository _walletRepository;

        private readonly IAccountingRepository<LedgerEntry> _ledgerEntryRepository;

        private readonly IPublishEndpoint _publishEndpoint;

        public TransferNetworkFundsIntegrationEventConsumer(IWalletRepository walletRepository, IAccountingRepository<LedgerEntry> ledgerEntryRepository, IPublishEndpoint publishEndpoint)
        {
            _walletRepository = walletRepository;
            _ledgerEntryRepository = ledgerEntryRepository;
            _publishEndpoint = publishEndpoint;
        }

        public async Task Consume(ConsumeContext<TransferNetworkFundsIntegrationEvent> context)
        {
            var senderWallet = await _walletRepository.SingleAsync(x => x.Id == context.Message.WalletId);

            var reciverWallet = await _walletRepository.SingleAsync(x => x.Id == context.Message.ReciverId);

            senderWallet.Depit(context.Message.Amount);
            reciverWallet.Credit(context.Message.Amount);

            var senderLedegerEntry = new LedgerEntry(senderWallet.Id, context.Message.Amount, TransferType.Network, TransferDirection.Depit, context.Message.TransferId, DateTime.UtcNow);

            var reciverLedgerEntry = new LedgerEntry(reciverWallet.Id, context.Message.Amount, TransferType.Network, TransferDirection.Credit, context.Message.TransferId, DateTime.UtcNow);

            await _walletRepository.UpdateAsync(senderWallet);

            await _walletRepository.UpdateAsync(reciverWallet);

            await _ledgerEntryRepository.InsertAsync(senderLedegerEntry);

            await _ledgerEntryRepository.InsertAsync(reciverLedgerEntry);

            var message = new TransferCompletedIntegrationEvent
            {
                TransferId = context.Message.TransferId,
                WalletId = context.Message.WalletId

            };

            await _publishEndpoint.Publish(message);
        }
    }
}
