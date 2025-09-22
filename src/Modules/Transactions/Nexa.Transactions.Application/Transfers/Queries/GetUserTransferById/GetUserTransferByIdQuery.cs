using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.Transactions.Application.Transfers.Dtos;

namespace Nexa.Transactions.Application.Transfers.Queries.GetUserTransferById
{
    [Authorize]
    public class GetUserTransferByIdQuery : IQuery<TransferDto>
    {
        public string TransferId { get; set; }
    }
}
