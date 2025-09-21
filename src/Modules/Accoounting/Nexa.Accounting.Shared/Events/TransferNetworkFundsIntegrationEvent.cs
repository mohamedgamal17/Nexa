namespace Nexa.Accounting.Shared.Events
{
    public class TransferNetworkFundsIntegrationEvent
    {
        public string UserId { get; set; }
        public string TransferId { get; set; }
        public string WalletId { get; set; }
        public string ReciverId { get; set; }
        public decimal Amount { get; set; }

        public TransferNetworkFundsIntegrationEvent(string userId,string transferId, string walletId, string reciverId, decimal amount)
        {
            UserId = userId;
            TransferId = transferId;
            WalletId = walletId;
            ReciverId = reciverId;
            Amount = amount;
        }
    }
}
