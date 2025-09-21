namespace Nexa.Accounting.Shared.Events
{
    public class TransferCompletedIntegrationEvent
    {
        public string TransferId  { get; set; }
        public string  WalletId { get; set; }
    }
}
