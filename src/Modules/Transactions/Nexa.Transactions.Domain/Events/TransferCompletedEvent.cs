using Nexa.BuildingBlocks.Domain.Events;
using Nexa.Transactions.Domain.Enums;

namespace Nexa.Transactions.Domain.Events
{
    public class TransferCompletedEvent : IEvent
    {
        public string Id { get; }
        public string WalletId { get; }
        public string Number { get; }
        public TransferType Type { get; }
        public DateTime CompletedAt { get; }

        public TransferCompletedEvent(string id, string walletId, string number, TransferType type, DateTime completedAt)
        {
            Id = id;
            WalletId = walletId;
            Number = number;
            Type = type;
            CompletedAt = completedAt;
        }
    }
}
