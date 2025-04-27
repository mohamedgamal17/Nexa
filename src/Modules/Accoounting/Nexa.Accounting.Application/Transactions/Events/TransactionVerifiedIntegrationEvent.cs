namespace Nexa.Accounting.Application.Transactions.Events
{
    public class TransactionVerifiedIntegrationEvent 
    {
        public string TransactionId { get; }
        public Type TransactionType { get; }
        public TransactionVerifiedIntegrationEvent(string transactionId, Type transactionType)
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
        }

    }

}
