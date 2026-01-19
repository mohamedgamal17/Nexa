using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
namespace Nexa.Accounting.Application.Wallets.Queries.GetWalletById
{
    [Authorize]
    public class GetWalletByIdQuery : IQuery<WalletListDto>
    {
        public string WalletId { get; set; }
    }

}
