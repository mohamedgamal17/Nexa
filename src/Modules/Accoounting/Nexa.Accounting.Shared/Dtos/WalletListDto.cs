using Nexa.Accounting.Shared.Enums;
using Nexa.BuildingBlocks.Domain.Dtos;
namespace Nexa.Accounting.Shared.Dtos
{
    public class WalletListDto : EntityDto
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string Number { get; set; }
    }
}
