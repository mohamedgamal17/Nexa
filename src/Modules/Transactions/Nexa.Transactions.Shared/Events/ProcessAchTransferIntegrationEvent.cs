using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Shared.Events
{
    public class ProcessAchTransferIntegrationEvent
    {
        public string TransferId { get; set; }
        public string WalletId { get; set; }
        public string CounterPartyId { get; set; }
        public decimal Amount { get; set; }
        public TransferDirection Direction { get; set; }

        public ProcessAchTransferIntegrationEvent(string transferId, string walletId, string counterPartyId, decimal amount, TransferDirection direction)
        {
            TransferId = transferId;
            WalletId = walletId;
            CounterPartyId = counterPartyId;
            Amount = amount;
            Direction = direction;
        }
    }
}
