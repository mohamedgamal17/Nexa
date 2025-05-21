using Nexa.Accounting.Domain.Enums;

namespace Nexa.Accounting.Application.Transactions.Events
{
    public class RequestTransactionVerificationIntgertationEvent
    {

        public string TransactionId { get; }
        public TransactionType TransactionType { get; }
        public RequestTransactionVerificationIntgertationEvent(string transactionId, TransactionType transactionType)
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
        }
    }

}
