using Nexa.Accounting.Domain.Enums;
using Nexa.BuildingBlocks.Domain.Events;

namespace Nexa.Accounting.Domain.Transactions.Events
{
    public class TransactionFailedEvent : IEvent
    {
        public string Id { get; }
        public string WalletId { get; }
        public string Number { get; }
        public TransactionType Type { get; }
        public TransactionFailedEvent(string id,
            string walletId,
            string transactionNumber,
            TransactionType type)
        {
            Id = id;
            WalletId = walletId;
            Number = transactionNumber;
            Type = type;
        }
    }
}
