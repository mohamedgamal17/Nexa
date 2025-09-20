using Nexa.BuildingBlocks.Domain.Events;
using Nexa.Transactions.Shared.Enums;
namespace Nexa.Transactions.Domain.Events
{
    public class TransferPendingEvent : IEvent
    {
        public string Id { get; }
        public string UserId { get; }
        public string WalletId { get; }
        public string Number { get; }
        public decimal Amount { get; }
        public TransferType Type { get; }

        public TransferPendingEvent(string id,
            string userId,
            string walletId,
            string transactionNumber,
            decimal amount,
            TransferType type)
        {
            Id = id;
            UserId = userId;
            WalletId = walletId;
            Number = transactionNumber;
            Amount = amount;
            Type = type;
        }
    }
}
