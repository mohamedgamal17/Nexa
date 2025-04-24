using Nexa.Accounting.Domain.Enums;
using Nexa.BuildingBlocks.Domain;

namespace Nexa.Accounting.Domain.Transactions
{
    public abstract class Transaction : BaseEntity
    {
        public string WalletId { get; set; }
        public string Number { get;protected set;}
        public decimal Amount { get;protected set; }
        public TransactionStatus Status { get; protected set; }

        protected Transaction(string walletId,
            string number,
            decimal amount) 
        {
            WalletId = walletId;
            Number = number;
            Amount = amount;
            Status = TransactionStatus.Pending;
        }
    }
}
