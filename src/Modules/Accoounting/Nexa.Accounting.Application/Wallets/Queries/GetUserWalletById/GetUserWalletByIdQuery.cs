using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.Accounting.Application.Wallets.Queries.GetUserWalletById
{
    [Authorize]
    public class GetUserWalletByIdQuery : IQuery<WalletDto>
    {
        public string WalletId { get; set; }
    }
}
