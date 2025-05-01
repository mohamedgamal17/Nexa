using Nexa.BuildingBlocks.Domain;

namespace Nexa.Accounting.Domain.Wallets
{
    public class WalletView : EntityView
    {
        public string Number { get;  set; }
        public string UserId { get;  set; }
        public decimal Balance { get;  set; }
    }
}
