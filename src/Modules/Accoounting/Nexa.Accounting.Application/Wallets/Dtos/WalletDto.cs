using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Application.Wallets.Dtos
{
    public class WalletDto : EntityDto
    {
        public string Number { get; private set; }
        public string UserId { get; private set; }
        public decimal Balance { get; private set; }
    }
}
