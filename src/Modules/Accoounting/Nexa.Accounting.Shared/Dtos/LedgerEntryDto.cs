using Nexa.Accounting.Shared.Enums;
using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Shared.Dtos
{
    public class LedgerEntryDto : EntityDto
    {
        public string WalletId { get;  set; }
        public decimal Amount { get;  set; }
        public string TransactionId { get;  set; }
        public TransferType Type { get;  set; }
        public TransferDirection Direction { get;  set; }
        public DateTime Timestamp { get;  set; }
    }
}
