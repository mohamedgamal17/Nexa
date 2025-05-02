using Nexa.Accounting.Application.Wallets.Dtos;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Wallets;
using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Application.Transactions.Dtos
{
    public class TransactionDto : EntityDto
    {
        public string WalletId { get; set; }
        public WalletDto Wallet { get; set; }
        public string Number { get;  set; }
        public decimal Amount { get;  set; }
        public string? ReciverId { get;  set; }
        public WalletDto? Reciver { get; set; }
        public string? PaymentId { get;  set; }
        public TransactionDirection? Direction { get;  set; }
        public TransactionStatus Status { get;  set; }
        public DateTime? CompletedAt { get;  set; }
        public TransactionType Type { get; set; }
    }
}
