using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Domain.Transfers
{

    public class BankTransfer : Transfer
    {

        public string CounterPartyId { get; protected set; }
        public TransferDirection Direction { get; protected set; }
        public BankTransferType BankTransferType { get; protected set; }

        protected BankTransfer()
        {
        }
        public BankTransfer(string walletId ,string number, decimal amount ,string counterPartyId, TransferDirection direction, BankTransferType bankTransferType) : base(walletId,number, amount)
        {
            CounterPartyId = counterPartyId;
            Direction = direction;
            BankTransferType = bankTransferType;
        }

        internal BankTransfer(string walletId, string number, decimal amount, string counterPartyId, TransferDirection direction, BankTransferType bankTransferType, TransferStatus status) : base(walletId, number, amount, status)
        {
            CounterPartyId = counterPartyId;
            Direction = direction;
            BankTransferType = bankTransferType;
        }

    }

    
}
