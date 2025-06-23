using Nexa.BuildingBlocks.Domain.Dtos;
namespace Nexa.Accounting.Shared.Dtos
{
    public class WalletDto : EntityDto
    {
        public string Number { get; set; }
        public string UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
