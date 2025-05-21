using Nexa.Accounting.Domain.Enums;
using Nexa.BuildingBlocks.Domain.Events;

namespace Nexa.Accounting.Domain.Transactions.Events
{
    public class TransactionCompletedEvent : IEvent
    {
        public string Id { get; }
        public string WalletId { get; }
        public string Number { get; }
        public TransactionType Type { get; }
        public DateTime CompletedAt { get;  }

        public TransactionCompletedEvent(string id, string walletId, string number, TransactionType type, DateTime completedAt)
        {
            Id = id;
            WalletId = walletId;
            Number = number;
            Type = type;
            CompletedAt = completedAt;
        }
    }
}
