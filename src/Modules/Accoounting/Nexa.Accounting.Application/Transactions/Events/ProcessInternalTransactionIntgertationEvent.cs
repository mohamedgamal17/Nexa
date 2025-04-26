namespace Nexa.Accounting.Application.Transactions.Events
{
    public class ProcessInternalTransactionIntgertationEvent
    {
        public string TransactionId { get; }

        public ProcessInternalTransactionIntgertationEvent(string transactionId)
        {
            TransactionId = transactionId;
        }
    }

}
