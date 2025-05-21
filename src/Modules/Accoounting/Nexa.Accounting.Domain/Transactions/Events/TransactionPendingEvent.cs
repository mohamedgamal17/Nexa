using Nexa.Accounting.Domain.Enums;
using Nexa.BuildingBlocks.Domain.Events;

namespace Nexa.Accounting.Domain.Transactions.Events
{
    public class TransactionPendingEvent : IEvent
    {
        public string Id { get; }
        public string WalletId { get;  }
        public string Number { get;  }
        public TransactionType Type { get;  }

        public TransactionPendingEvent(string id, 
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
