namespace Nexa.Transactions.Shared.Events
{
    public class ProcessNetworkTransferIntegrationEvent 
    {
        public string TransferId { get; set; }
        public string Number { get; set; }
        public string WalletId { get; set; }
        public string ReciverId { get; set; }
        public decimal Amount { get; set; }

        public ProcessNetworkTransferIntegrationEvent(string transferId, string number, string walletId, string reciverId, decimal amount)
        {
            TransferId = transferId;
            Number = number;
            WalletId = walletId;
            ReciverId = reciverId;
            Amount = amount;
        }
    }
}
