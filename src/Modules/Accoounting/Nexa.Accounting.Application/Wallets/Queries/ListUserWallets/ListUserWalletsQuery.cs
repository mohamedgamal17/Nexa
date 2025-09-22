using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Application.Wallets.Queries.ListUserWallets
{
    [Authorize]
    public class ListUserWalletsQuery : PagingParams, IQuery<Paging<WalletDto>>
    {

    }
}
