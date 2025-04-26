using Nexa.Accounting.Application.Transactions.Dtos;
using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.Accounting.Application.Wallets.Commands.Transfer
{
    public class TransferCommand : ICommand<TransactionDto>
    {
        public string SenderId { get; set; }
        public string ReciverId { get; set; }
        public decimal Amount { get; set; }
    }
}
