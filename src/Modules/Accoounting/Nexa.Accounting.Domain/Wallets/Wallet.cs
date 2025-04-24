using Nexa.BuildingBlocks.Domain;

namespace Nexa.Accounting.Domain.NewFolder
{
    public class Wallet : BaseEntity
    {
        public string Number { get; private set; }
        public string UserId { get;private set; }
        public decimal Balance { get; private set; }

        public Wallet(string number , string userId)
        {
            Number = number;
            UserId = userId;
        }
        public void UpdateBalance(decimal newBalance)
        {
            Balance = newBalance;
        }
    }

}
