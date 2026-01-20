using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Shared.Dtos
{
    public class TransferDto : EntityDto
    {
        public string UserId { get; set; }
        public string WalletId { get; set; }
        public WalletListDto Wallet { get; set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public string? ReciverId { get; set; }
        public WalletListDto? Reciver { get; set; }
        public string? FundingResourceId { get; set; }
        public BankAccountDto? FundingResource { get; set; }
        public TransferDirection? Direction { get; set; }
        public BankTransferType? BankTransferType { get; set; }
        public TransferStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TransferType Type { get; set; }
    }
}
