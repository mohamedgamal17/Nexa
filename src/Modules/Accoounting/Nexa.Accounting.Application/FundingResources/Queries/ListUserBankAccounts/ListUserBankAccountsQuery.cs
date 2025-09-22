using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Application.FundingResources.Queries.ListUserBankAccounts
{
    [Authorize]
    public class ListUserBankAccountsQuery : PagingParams ,IQuery<Paging<BankAccountDto>>
    {

    }
}
