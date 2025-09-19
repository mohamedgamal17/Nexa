using Nexa.BuildingBlocks.Domain;
using Nexa.Transactions.Domain.Events;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Domain.Transfers
{
    public abstract class Transfer : AggregateRoot
    {
        public string UserId { get; protected set; }
        public string WalletId { get; protected set; }
        public string Number { get; protected set; }
        public decimal Amount { get; protected set; }
        public TransferStatus Status { get; protected set; }
        public DateTime? CompletedAt { get;protected set; }
        public TransferType Type { get; protected set; }
        protected Transfer() { }

        protected Transfer(string userId, string walletId, string number, decimal amount)
        {
            UserId = userId;
            WalletId = walletId;
            Number = number;
            Amount = amount;
        }
        protected Transfer(string walletId, string number, decimal amount)
        {
            WalletId = walletId;
            Number = number;
            Amount = amount;
            var @event = new TransferPendingEvent(Id, WalletId, Number, Amount, Type);
            AppendEvent(@event);
        }

        internal Transfer(string walletId, string number, decimal amount, TransferStatus status)
        {
            WalletId = walletId;
            Number = number;
            Amount = amount;
            Status = status;
        }
        public void Process()
        {
            if (Status != TransferStatus.Pending)
            {
                throw new InvalidOperationException($"Transaction cannot move to process state beacuse current state ({Status.ToString()}), is invalid state");
            }


            Status = TransferStatus.Processing;

            var @event = new TransferProcessingEvent(Id, WalletId, Number, Type);

            AppendEvent(@event);
        }


        public void Complete()
        {
            if (Status != TransferStatus.Processing)
            {
                throw new InvalidOperationException($"Transaction cannot move to process state beacuse current state ({Status.ToString()}), is invalid state");
            }

            Status = TransferStatus.Completed;

            CompletedAt = DateTime.UtcNow;

            var @event = new TransferCompletedEvent(Id, WalletId, Number, Type, CompletedAt.Value);

            AppendEvent(@event);
        }

        public void Cancel()
        {
            if (Status == TransferStatus.Completed)
            {
                throw new InvalidOperationException($"Transaction cannot move to process state beacuse current state ({Status.ToString()}), is invalid state");
            }

            Status = TransferStatus.Faild;


        }
    }
}
