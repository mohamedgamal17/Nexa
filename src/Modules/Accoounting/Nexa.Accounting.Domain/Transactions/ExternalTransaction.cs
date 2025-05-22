using Nexa.Accounting.Domain.Enums;
namespace Nexa.Accounting.Domain.Transactions
{
    public class ExternalTransaction : Transaction
    {
        public string PaymentId { get; private set; }
        public TransactionDirection Direction { get; private set; }


        //Constructor for efcore
        private ExternalTransaction()
        {

        }
        // Internal constructor for testing purpose only
        internal ExternalTransaction(string walletId, string paymentId, string number, decimal amount,TransactionDirection direction ,TransactionStatus status)
           : base(walletId, number, amount, status)
        {
            PaymentId = paymentId;
            Direction = direction;

        }

        public ExternalTransaction(string walletId, 
            string number, 
            decimal amount , 
            string paymentId,
            TransactionDirection direction) : base(walletId, number, amount)
        {
            PaymentId = paymentId;
            Direction = direction;
        }

     
    }
}
