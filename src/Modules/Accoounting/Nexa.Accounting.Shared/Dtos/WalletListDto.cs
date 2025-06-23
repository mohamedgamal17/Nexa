using Nexa.BuildingBlocks.Domain.Dtos;
namespace Nexa.Accounting.Shared.Dtos
{
    public class WalletListDto : EntityDto
    {
        public string Number { get; set; }
        public string UserId { get; set; }
    }
}
