using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Application.Wallets.Queries.ListWallets
{
    [Authorize]
    public class ListWalletQuery : PagingParams , IQuery<Paging<WalletListDto>>
    {
        public string? Number { get; set; }
    }

    public class ListWalletQueryValidator : AbstractValidator<ListWalletQuery>
    {
        public ListWalletQueryValidator()
        {
            Include(new PagingParamasValidator<ListWalletQuery>());

            RuleFor(x => x.Number)
                .MaximumLength(256)
                .When(x => x.Number != null);

        }
    }
}
