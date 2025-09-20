namespace Nexa.Accounting.Shared.Events
{
    public class ReciveBalanceIntegrationEvent
    {
        public string WalletId { get; set; }
        public string TransferId { get; set; }
        public string TransferNumber { get; set; }
        public decimal Amount { get; set; }
        public string FundingResourceId { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
