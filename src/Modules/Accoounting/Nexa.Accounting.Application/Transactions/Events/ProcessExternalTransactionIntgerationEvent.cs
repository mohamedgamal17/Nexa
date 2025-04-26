namespace Nexa.Accounting.Application.Transactions.Events
{
    public class ProcessExternalTransactionIntgerationEvent
    {
        public string TransactionId { get; }

        public ProcessExternalTransactionIntgerationEvent(string transactionId)
        {
            TransactionId = transactionId;
        }
    }

}
