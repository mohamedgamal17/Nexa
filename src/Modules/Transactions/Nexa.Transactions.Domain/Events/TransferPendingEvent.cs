using Nexa.BuildingBlocks.Domain.Events;
using Nexa.Transactions.Domain.Enums;
namespace Nexa.Transactions.Domain.Events
{
    public class TransferPendingEvent : IEvent
    {
        public string Id { get; }
        public string WalletId { get; }
        public string Number { get; }
        public TransferType Type { get; }

        public TransferPendingEvent(string id,
            string walletId,
            string transactionNumber,
            TransferType type)
        {
            Id = id;
            WalletId = walletId;
            Number = transactionNumber;
            Type = type;
        }
    }
}
