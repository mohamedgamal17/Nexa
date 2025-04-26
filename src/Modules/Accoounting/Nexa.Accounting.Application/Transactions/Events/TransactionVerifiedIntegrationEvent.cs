namespace Nexa.Accounting.Application.Transactions.Events
{
    public class TransactionVerifiedIntegrationEvent 
    {
        public string TransactionId { get; }
        public string TransactionType { get; }
        public TransactionVerifiedIntegrationEvent(string transactionId, string transactionType)
        {
            TransactionId = transactionId;
            TransactionType = transactionType;
        }

    }

}
