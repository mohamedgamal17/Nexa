namespace Nexa.Accounting.Application.Transactions.Events
{
    public class RequestTransactionVerificationIntgertationEvent
    {

        public string TransactionId { get; }
        public Type TransactionType { get; }
        public RequestTransactionVerificationIntgertationEvent(string transactionId, Type transactionType)
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
        }
    }

}
