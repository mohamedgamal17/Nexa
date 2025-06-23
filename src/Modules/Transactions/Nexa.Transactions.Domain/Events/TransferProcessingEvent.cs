using Nexa.BuildingBlocks.Domain.Events;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Domain.Events
{
    public class TransferProcessingEvent : IEvent
    {

        public string Id { get; }
        public string WalletId { get; }
        public string Number { get; }
        public TransferType Type { get; }
        public TransferProcessingEvent(string id, string walletId, string number, TransferType type)
        {
            Id = id;
            WalletId = walletId;
            Number = number;
            Type = type;
        }
    }
}
