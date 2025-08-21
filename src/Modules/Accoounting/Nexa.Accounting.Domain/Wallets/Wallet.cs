using Ardalis.GuardClauses;
using Nexa.Accounting.Shared.Enums;
using Nexa.BuildingBlocks.Domain;

namespace Nexa.Accounting.Domain.Wallets
{
    public class Wallet : BaseEntity
    {
        public string CustomerId { get; private set; }
        public string UserId { get; private set; }
        public string Number { get; private set; }
        public decimal Balance { get; private set; }
        public decimal ReservedBalance { get; private set; }
        public WalletState State { get; private set; }

        //Constructor for efcore
        private Wallet() { }
        public Wallet(string customerId, string userId,string number)
        {
            CustomerId = customerId;
            UserId = userId;
            Number = number;
           
        }

        //internal constructor for testing purpose only
        internal Wallet(string customerId, string userId,string number , decimal balance) 
        {
            CustomerId = customerId;
            UserId = userId;
            Number = number;        
            Balance = balance;
        }


        public void Depit(decimal amount)
        {
            Guard.Against.NegativeOrZero(amount);

            if (ReservedBalance < amount)
            {
                throw new InvalidOperationException($"Insufficient reserved balance : ({ReservedBalance}) , cannot relase amount : ({amount}) .");
            }

            ReservedBalance -= amount;
        }


        public void Credit(decimal amount)
        {
            Guard.Against.NegativeOrZero(amount);

            Balance += amount;
        }


        public void Reserve(decimal amount)
        {
            Guard.Against.NegativeOrZero(amount);

            if (Balance < amount)
            {
                throw new InvalidOperationException("Insufficient balance to reserved");
            }

            Balance -= amount;

            ReservedBalance += amount; 
        }

        public void Release(decimal amount)
        {
            Guard.Against.NegativeOrZero(amount);

            if (ReservedBalance < amount)
            {
                throw new InvalidOperationException($"Insufficient reserved balance : ({ReservedBalance}) , cannot relase amount : ({amount}) .");
            }

            ReservedBalance -= amount;

            Balance += amount;
        }
    }

}
