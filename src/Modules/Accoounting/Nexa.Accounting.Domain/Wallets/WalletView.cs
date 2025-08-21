using Nexa.Accounting.Shared.Enums;
using Nexa.BuildingBlocks.Domain;

namespace Nexa.Accounting.Domain.Wallets
{
    public class WalletView : EntityView
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string Number { get;  set; }    
        public decimal Balance { get;  set; }
        public WalletState State { get; set; }
    }
}
