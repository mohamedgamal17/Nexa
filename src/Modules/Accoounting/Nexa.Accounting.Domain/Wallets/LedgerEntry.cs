using Nexa.Accounting.Domain.Enums;
using Nexa.BuildingBlocks.Domain;
namespace Nexa.Accounting.Domain.Wallets
{
    public class LedgerEntry : BaseEntity
    {
        public string WalletId { get; private set; }
        public decimal Amount { get; private set; }
        public string TransactionId { get; private set; }
        public LedgerEntryType Type { get; private set; }
        public TransactionDirection Direction { get; private set; }
        public DateTime Timestamp { get; private set; }
        public LedgerEntry(string walletId,
            decimal amount,
            LedgerEntryType type,
            TransactionDirection direction,
            string transactionId,
            DateTime timestamp)
        {
            WalletId = walletId;
            Amount = amount;
            Timestamp = timestamp;
            Type = type;
            TransactionId = transactionId;
            Direction = direction;
        }
    }

}
