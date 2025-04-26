using Nexa.Accounting.Domain.Enums;
using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Application.Transactions.Dtos
{
    public class TransactionDto : EntityDto
    {
        public string WalletId { get; set; }
        public string Number { get;  set; }
        public decimal Amount { get;  set; }
        public string? ReciverId { get; private set; }
        public string? PaymentId { get; private set; }
        public TransactionDirection? Direction { get; private set; }
        public TransactionStatus Status { get;  set; }
        public DateTime? CompletedAt { get;  set; }
        public TransactionType Type { get; set; }
    }
}
