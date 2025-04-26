using Nexa.BuildingBlocks.Domain.Events;

namespace Nexa.Accounting.Domain.Transactions.Events
{
    public class TransactionCompletedEvent : IEvent
    {
        public string Id { get; }
        public string WalletId { get; }
        public string Number { get; }
        public Type Type { get; }

        public DateTime CompletedAt { get;  }

        public TransactionCompletedEvent(string id, string walletId, string number, Type type, DateTime completedAt)
        {
            Id = id;
            WalletId = walletId;
            Number = number;
            Type = type;
            CompletedAt = completedAt;
        }
    }
}
