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
        public BankTransfer(string counterPartyId, TransferDirection direction, BankTransferType bankTransferType)
        {
            CounterPartyId = counterPartyId;
            Direction = direction;
            BankTransferType = bankTransferType;
        }


    }

    
}
