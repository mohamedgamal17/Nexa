using Nexa.Accounting.Shared.Enums;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.Accounting.Shared.Dtos
{
    public class WalletListDto : EntityDto
    {
        public string ProviderWalletId { get; set; }
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string Number { get; set; }
        public WalletState State { get; set; }
        public CustomerPublicDto? Customer { get; set; }

    }
}
