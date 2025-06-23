using MediatR;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.Transactions.Application.Transfers.Dtos;

namespace Nexa.Transactions.Application.Transfers.Commands.CreateNetworkTransfer
{
    public class CreateNetworkTransferCommand : ICommand<TransferDto>
    {
        public string SenderId { get; set; }
        public string ReciverId { get; set; }
        public decimal Amount { get; set; }
    }
}
