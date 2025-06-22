using Nexa.Transactions.Domain.Enums;

namespace Nexa.Transactions.Domain.Transfers
{
    public class NetworkTransfer : Transfer
    {
        public string ReciverId { get; protected set; }

        private NetworkTransfer() { }
     
        public NetworkTransfer(string walletId, string reciverId ,string number, decimal amount) 
            : base(walletId, number, amount)
        {
            ReciverId = reciverId;
        }

        internal NetworkTransfer(string walletId, string reciverId, string number, decimal amount , TransferStatus status)
              : base(walletId, number, amount , status)
        {
            ReciverId = reciverId;
        }
    }
}
