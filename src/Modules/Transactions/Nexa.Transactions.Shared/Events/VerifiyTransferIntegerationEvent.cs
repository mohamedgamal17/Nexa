using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Shared.Events
{
    public class VerifiyTransferIntegerationEvent
    {
        public string TransferId { get; set; }
        public string TransferNumber { get; set; }
        public string WalletId { get; set; }
        public decimal Amount { get; set; }
        public TransferType Type { get; set; }
        public VerifiyTransferIntegerationEvent(string transferId, string transferNumber, string walletId, decimal amount, TransferType type)
        {
            TransferId = transferId;
            TransferNumber = transferNumber;
            WalletId = walletId;
            Amount = amount;
            Type = type;
        }
    }
}
