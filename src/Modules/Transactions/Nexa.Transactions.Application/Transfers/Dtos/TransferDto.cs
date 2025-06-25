using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Application.Transfers.Dtos
{
    public class TransferDto : EntityDto
    {
        public string WalletId { get; set; }
        public WalletListDto Wallet { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public string? ReciverId { get; set; }
        public WalletListDto? Reciver { get; set; }
        public string? CounterPartyId { get; set; }
        public TransferDirection? Direction { get; set; }
        public TransferStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TransferType Type { get; set; }
    }
}
