using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Wallets;
using Nexa.BuildingBlocks.Domain;

namespace Nexa.Accounting.Domain.Transactions
{
    public class TransactionView : EntityView
    {
        public string WalletId { get;  set; }
        public WalletView Wallet { get; set; }
        public string Number { get;  set; }
        public decimal Amount { get;  set; }
        public string? ReciverId { get; set; }
        public WalletView? Reciver { get; set; }
        public string? PaymentId { get; set; }
        public TransactionStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TransactionType Type { get; set; }
    }
}
