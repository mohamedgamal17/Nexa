using Nexa.BuildingBlocks.Domain;
using Nexa.Transactions.Domain.Enums;

namespace Nexa.Transactions.Domain.Transfers
{
    public abstract class Transfer : AggregateRoot
    {
        public string WalletId { get; protected set; }
        public string Number { get; protected set; }
        public decimal Amount { get; protected set; }
        public TransferStatus Status { get; protected set; }
        public DateTime? CompletedAt { get;protected set; }
        public TransferType Type { get; protected set; }
        protected Transfer() { }
        protected Transfer(string walletId, string number, decimal amount)
        {
            WalletId = walletId;
            Number = number;
            Amount = amount;
        }

        internal Transfer(string walletId, string number, decimal amount, TransferStatus status)
        {
            WalletId = walletId;
            Number = number;
            Amount = amount;
            Status = status;
        }
    }
}
