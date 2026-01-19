using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Application.Wallets.Queries.ListUserWallets
{
    [Authorize]
    public class ListUserWalletsQuery : PagingParams, IQuery<Paging<WalletDto>>
    {
        public string? Number { get; set; }
    }

    public class ListUserWalletsQueryValidator : AbstractValidator<ListUserWalletsQuery>
    {
        public ListUserWalletsQueryValidator()
        {
            Include(new PagingParamasValidator<ListUserWalletsQuery>());

            RuleFor(x => x.Number)
                .MaximumLength(256)
                .When(x => x.Number != null);
        }
    }
}
