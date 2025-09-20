using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Shared.Events
{
    public class ProcessBankTransferIntegrationEvent
    {
        public string  UserId { get; set; }
        public string TransferId { get; set; }
        public string WalletId { get; set; }
        public string FundingResourceId { get; set; }
        public decimal Amount { get; set; }
        public TransferDirection Direction { get; set; }
        public BankTransferType BankTransferType { get; set; }

        public ProcessBankTransferIntegrationEvent(string userId,string transferId, string walletId, string fundingResourceId, decimal amount, TransferDirection direction, BankTransferType bankTransferType)
        {
            UserId = userId;
            TransferId = transferId;
            WalletId = walletId;
            FundingResourceId = fundingResourceId;
            Amount = amount;
            Direction = direction;
            BankTransferType = bankTransferType;
        }
    }
}
