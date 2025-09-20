using Nexa.Transactions.Domain.Events;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Domain.Transfers
{
    public class NetworkTransfer : Transfer
    {
        public string ReciverId { get; protected set; }

        private NetworkTransfer() { }
     
        public NetworkTransfer(string userId,string walletId, string reciverId ,string number, decimal amount) 
            : base(userId,walletId, number, amount)
        {
            ReciverId = reciverId;

            var @event = new NetworkTransferInitiatedEvent(Id, Number, WalletId, reciverId, Amount);

            AppendEvent(@event);
        }

        internal NetworkTransfer(string walletId, string reciverId, string number, decimal amount , TransferStatus status)
              : base(walletId, number, amount , status)
        {
            ReciverId = reciverId;
        }
    }
}
