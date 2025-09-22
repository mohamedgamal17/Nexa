using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.Transactions.Application.Transfers.Dtos;

namespace Nexa.Transactions.Application.Transfers.Queries.ListUserTransfers
{
    [Authorize]
    public class ListUserTransfersQuery : PagingParams , IQuery<Paging<TransferDto>> 
    {
        public string? WalletId { get; set; }
    }
}
