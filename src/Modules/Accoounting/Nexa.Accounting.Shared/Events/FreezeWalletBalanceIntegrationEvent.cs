namespace Nexa.Accounting.Shared.Events
{
    public class FreezeWalletBalanceIntegrationEvent
    {
        public string TransferId { get; set; }
        public string WalletId { get; set; }
        public decimal Amount { get; set; }
    }
}
