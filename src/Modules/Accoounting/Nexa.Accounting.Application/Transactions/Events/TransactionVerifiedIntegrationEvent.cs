using Nexa.Accounting.Domain.Enums;

namespace Nexa.Accounting.Application.Transactions.Events
{
    public class TransactionVerifiedIntegrationEvent 
    {
        public string TransactionId { get; }
        public TransactionType TransactionType { get; }
        public TransactionVerifiedIntegrationEvent(string transactionId, TransactionType transactionType)
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
        }

    }

}
