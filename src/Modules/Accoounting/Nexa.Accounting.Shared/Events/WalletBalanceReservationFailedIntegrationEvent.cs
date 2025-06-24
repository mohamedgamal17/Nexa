namespace Nexa.Accounting.Shared.Events
{
    public class WalletBalanceReservationFailedIntegrationEvent
    {
        public string TransferId { get; set; }
        public string WalletId { get; set; }
        public string Reason { get; set; }
        public decimal  Amount { get; set; }
    }
}
