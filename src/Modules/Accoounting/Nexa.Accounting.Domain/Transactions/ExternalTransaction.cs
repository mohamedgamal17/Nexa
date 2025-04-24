using Nexa.Accounting.Domain.Enums;
namespace Nexa.Accounting.Domain.Transactions
{
    public class ExternalTransaction : Transaction
    {
        public string PaymentId { get; private set; }
        public TransactionDirection Direction { get; private set; }
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
