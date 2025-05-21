using Nexa.Accounting.Domain.Enums;
using Nexa.BuildingBlocks.Domain.Events;

namespace Nexa.Accounting.Domain.Transactions.Events
{
    public class TransactionProcessingEvent : IEvent
    {
 
        public string Id { get;  }
        public string WalletId { get;  }
        public string Number { get;  }
        public TransactionType Type { get;  }
        public TransactionProcessingEvent(string id, string walletId, string number, TransactionType type)
        {
            Id = id;
            WalletId = walletId;
            Number = number;
            Type = type;
        }
    }
}
