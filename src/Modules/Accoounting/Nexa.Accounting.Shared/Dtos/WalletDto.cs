using Nexa.Accounting.Shared.Enums;
using Nexa.BuildingBlocks.Domain.Dtos;
namespace Nexa.Accounting.Shared.Dtos
{
    public class WalletDto : EntityDto
    {
        public string ProviderWalletId { get;  set; }
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string Number { get; set; } 
        public decimal Balance { get; set; }
        public WalletState State { get; set; }
    }
}
