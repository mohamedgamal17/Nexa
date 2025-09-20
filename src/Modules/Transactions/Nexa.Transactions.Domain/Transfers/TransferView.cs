using Nexa.BuildingBlocks.Domain;
using Nexa.Transactions.Shared.Enums;
namespace Nexa.Transactions.Domain.Transfers
{
    public class TransferView : EntityView
    {
        public string UserId { get; set; }
        public string WalletId { get; set; }
        public string? ExternalTransferId { get; protected set; }
        public string Number { get; set; }
        public decimal Amount { get; set; }
        public string? ReciverId { get; set; }
        public string? FundingResourceId { get; set; }
        public TransferDirection? Direction { get; set; }
        public BankTransferType? BankTransferType { get; set; }
        public TransferStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TransferType Type { get; set; }
    }
}
