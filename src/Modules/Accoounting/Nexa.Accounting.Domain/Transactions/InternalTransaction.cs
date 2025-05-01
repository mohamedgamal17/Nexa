using Nexa.Accounting.Domain.Enums;

namespace Nexa.Accounting.Domain.Transactions
{
    public class InternalTransaction : Transaction
    {
        public string ReciverId { get; private set; }

       //Constructor for efcore
        private InternalTransaction()
        {

        }
        public InternalTransaction(string walletId , 
            string number ,          
            decimal amount ,
            string reciverId ) : base(walletId, number, amount)
        {
            ReciverId = reciverId;

        }

        // Internal constructor for testing purpose only
        internal InternalTransaction(string walletId, string reciverId ,string number, decimal amount, TransactionStatus status)
            : base(walletId,number, amount,status)
        {
            ReciverId = reciverId;
        }
    }  
}
