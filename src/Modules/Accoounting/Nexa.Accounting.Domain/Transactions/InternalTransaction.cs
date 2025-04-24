namespace Nexa.Accounting.Domain.Transactions
{
    public class InternalTransaction : Transaction
    {
        public string ReciverId { get; private set; }

        public InternalTransaction(string walletId , 
            string number ,          
            decimal amount ,
            string reciverId ) : base(walletId, number, amount)
        {
            ReciverId = reciverId;

        }
    }


    

   
}
