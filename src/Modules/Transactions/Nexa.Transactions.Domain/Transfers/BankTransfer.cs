using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Domain.Transfers
{
    public class BankTransfer : Transfer
    {
        public string FundingResourceId { get; protected set; }
        public TransferDirection Direction { get; protected set; }
        public BankTransferType BankTransferType { get; protected set; }

        protected BankTransfer()
        {
        }
        public BankTransfer(string userId ,string walletId ,string number, decimal amount ,string fundingResourceId, TransferDirection direction, BankTransferType bankTransferType) : base(userId, walletId,number, amount)
        {
            FundingResourceId = fundingResourceId;
            Direction = direction;
            BankTransferType = bankTransferType;
        }

        internal BankTransfer(string walletId, string number, decimal amount, string fundingResourceId, TransferDirection direction, BankTransferType bankTransferType, TransferStatus status) : base(walletId, number, amount, status)
        {
            FundingResourceId = fundingResourceId;
            Direction = direction;
            BankTransferType = bankTransferType;
        }

        public static BankTransfer Deposit(string userId, string walletId, string number, decimal amount, string fundingResourceId)
        {
            return new BankTransfer(userId, walletId, number, amount, fundingResourceId, TransferDirection.Credit, BankTransferType.Ach);
        }

        public static BankTransfer Withdraw(string userId, string walletId, string number, decimal amount, string fundingResourceId)
        {
            return new BankTransfer(userId, walletId, number, amount, fundingResourceId, TransferDirection.Depit, BankTransferType.Ach);
        }
    }

    
}
