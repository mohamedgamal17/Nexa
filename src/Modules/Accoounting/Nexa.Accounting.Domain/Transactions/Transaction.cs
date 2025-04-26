using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Transactions.Events;
using Nexa.BuildingBlocks.Domain;

namespace Nexa.Accounting.Domain.Transactions
{
    public abstract class Transaction : AggregateRoot
    {
        public string WalletId { get; set; }
        public string Number { get;protected set;}
        public decimal Amount { get;protected set; }
        public TransactionStatus Status { get; protected set; }

        protected Transaction(string walletId,
            string number,
            decimal amount) 
        {
            WalletId = walletId;
            Number = number;
            Amount = amount;
            Status = TransactionStatus.Pending;
            var @event = new TransactionPendingEvent(Id, walletId, number, this.GetType());
            AppendEvent(@event);
        }

        public void Process()
        {
            if(Status != TransactionStatus.Pending)
            {
                throw new InvalidOperationException($"Transaction cannot move to process state beacuse current state ({Status.ToString()}), is invalid state");
            }


            Status = TransactionStatus.Processing;

            var @event = new TransactionProcessingEvent(Id, WalletId, Number, this.GetType());

            AppendEvent(@event);
        }


        public void Complete()
        {
            if (Status != TransactionStatus.Processing)
            {
                throw new InvalidOperationException($"Transaction cannot move to process state beacuse current state ({Status.ToString()}), is invalid state");
            }

            Status = TransactionStatus.Completed;

            var @event = new TransactionCompletedEvent(Id, WalletId, Number, this.GetType());

            AppendEvent(@event);
        }

        public void Cancel()
        {
            if(Status != TransactionStatus.Completed)
            {
                throw new InvalidOperationException($"Transaction cannot move to process state beacuse current state ({Status.ToString()}), is invalid state");
            }

            Status = TransactionStatus.Faild;


        }
    }
}
