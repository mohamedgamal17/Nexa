using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Domain.Transfers
{
    public class WireTransfer : Transfer
    {
        public string CounterPartyId { get; protected set; }
        public TransferDirection Direction { get; protected set; }

        public WireTransfer() { }
     
        public WireTransfer(string walletId, string number, decimal amount, string counterPartyId, TransferDirection direction)
            : base(walletId, number, amount)
        {
            CounterPartyId = counterPartyId;
            Direction = direction;
        }

        internal WireTransfer(string walletId, string number, decimal amount, string counterPartyId, TransferDirection direction, TransferStatus status)
         : base(walletId, number, amount, status)
        {
            CounterPartyId = counterPartyId;
            Direction = direction;
        }
    }
}
