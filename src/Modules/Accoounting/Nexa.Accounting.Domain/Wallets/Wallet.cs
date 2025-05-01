using Ardalis.GuardClauses;
using Nexa.BuildingBlocks.Domain;

namespace Nexa.Accounting.Domain.Wallets
{
    public class Wallet : BaseEntity
    {
        public string Number { get; private set; }
        public string UserId { get; private set; }
        public decimal Balance { get; private set; }

        //Constructor for efcore
        private Wallet() { }
        public Wallet(string number, string userId)
        {
            Number = number;
            UserId = userId;
        }

        //internal constructor for testing purpose only
        internal Wallet(string number , string userId, decimal balance) 
        {
            Number = number;
            UserId = userId;
            Balance = balance;
        }


        public void Depit(decimal amount)
        {
            Guard.Against.NegativeOrZero(amount);

            if(Balance < amount)
            {
                throw new InvalidOperationException("Insufficient balance to complete the depit operation.");
            }

            Balance -= amount;
        }


        public void Credit(decimal amount)
        {
            Guard.Against.NegativeOrZero(amount);

            Balance += amount;
        }
    }

}
