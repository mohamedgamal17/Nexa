using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Shared.Events
{
    public class ProcessWireTransferIntegrationEvent
    {
        public string TransferId { get; set; }
        public string WalletId { get; set; }
        public string CounterPartyId { get; set; }
        public decimal Amount { get; set; }
        public TransferDirection Direction { get; set; }
        public ProcessWireTransferIntegrationEvent(string transferId, string walletId, string counterPartyId, decimal amount, TransferDirection direction)
        {
            TransferId = transferId;
            WalletId = walletId;
            CounterPartyId = counterPartyId;
            Amount = amount;
            Direction = direction;
        }

    }
}
