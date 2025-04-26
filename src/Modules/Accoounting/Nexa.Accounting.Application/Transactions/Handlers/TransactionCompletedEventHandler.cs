using MediatR;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Transactions;
using Nexa.Accounting.Domain.Transactions.Events;
using Nexa.Accounting.Domain.Wallets;
namespace Nexa.Accounting.Application.Transactions.Handlers
{
    public class TransactionCompletedEventHandler : INotificationHandler<TransactionCompletedEvent>
    {
        private readonly IAccountingRepository<Transaction> _transactionRepository;
        private readonly IAccountingRepository<Wallet> _walletRepository;
        private readonly IAccountingRepository<LedgerEntry> _ledgerEntryRepository;

        public TransactionCompletedEventHandler(IAccountingRepository<Transaction> transactionRepository, IAccountingRepository<Wallet> walletRepository, IAccountingRepository<LedgerEntry> ledgerEntryRepository)
        {
            _transactionRepository = transactionRepository;
            _walletRepository = walletRepository;
            _ledgerEntryRepository = ledgerEntryRepository;
        }

        public async Task Handle(TransactionCompletedEvent notification, CancellationToken cancellationToken)
        {
            var transaction = await _transactionRepository.SingleAsync(x => x.Id == notification.Id);

            if(notification.Type == typeof(ExternalTransaction))
            {
                await HandlerExtertnalTransaction((ExternalTransaction)transaction, notification);
            }
            else
            {
                await HandleInternalTransaction((InternalTransaction)transaction, notification);
            }
        }

        private async Task HandleInternalTransaction(InternalTransaction transaction , TransactionCompletedEvent notification)
        {
            var senderWallet = await _walletRepository.SingleAsync(x => x.Id == transaction.WalletId);

            var reciverWallet = await _walletRepository.SingleAsync(x => x.Id == transaction.ReciverId);

            var senderLedegerEntry = new LedgerEntry(senderWallet.Id, transaction.Amount, TransactionType.Internal, TransactionDirection.Depit, transaction.Id, notification.CompletedAt);

            var reciverLedgerEntry = new LedgerEntry(reciverWallet.Id, transaction.Amount, TransactionType.Internal, TransactionDirection.Credit, transaction.Id, notification.CompletedAt);

            await _ledgerEntryRepository.InsertManyAsync(new List<LedgerEntry> { senderLedegerEntry, reciverLedgerEntry });

        }

        private async Task HandlerExtertnalTransaction(ExternalTransaction transaction, TransactionCompletedEvent notification)
        {
            var wallet = await _walletRepository.SingleAsync(x => x.Id == transaction.WalletId);

            var direction = transaction.Direction;

            var ledgerEntry = new LedgerEntry(wallet.Id, transaction.Amount, TransactionType.External, direction, transaction.Id, notification.CompletedAt);

            await _ledgerEntryRepository.InsertAsync(ledgerEntry);
        }
    }
}
