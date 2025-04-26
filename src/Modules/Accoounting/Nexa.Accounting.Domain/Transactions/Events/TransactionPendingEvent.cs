using Nexa.BuildingBlocks.Domain.Events;

namespace Nexa.Accounting.Domain.Transactions.Events
{
    public class TransactionPendingEvent : IEvent
    {
        public string Id { get; }
        public string WalletId { get;  }
        public string Number { get;  }
        public Type Type { get;  }

        public TransactionPendingEvent(string id, 
            string walletId, 
            string transactionNumber,
            Type type)
        {
            Id = id;
            WalletId = walletId;
            Number = transactionNumber;
            Type = type;
        }
    }
}
