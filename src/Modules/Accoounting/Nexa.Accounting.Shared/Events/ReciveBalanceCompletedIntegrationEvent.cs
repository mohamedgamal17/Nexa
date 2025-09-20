namespace Nexa.Accounting.Shared.Events
{
    public class ReciveBalanceCompletedIntegrationEvent
    {
        public string WalletId { get; set; }
        public string TransferId { get; set; }
    }
}
