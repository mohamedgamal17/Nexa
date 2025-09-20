namespace Nexa.Transactions.Shared.Events
{
    public class ExternalTransferCompletedIntegrationEvent
    {
        public string TransferId { get; set; }
        public string ExternalTransferId { get; set; }
    }
}
