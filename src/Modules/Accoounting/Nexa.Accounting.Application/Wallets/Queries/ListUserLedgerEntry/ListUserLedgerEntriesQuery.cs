using FluentValidation;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Factories;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Application.Wallets.Queries.ListUserLedgerEntry
{
    public class ListUserLedgerEntriesQuery : PagingParams ,IQuery<Paging<LedgerEntryDto>>
    {
        public string WalletId { get; set; }
    }

    public class ListUserLedgerEntiresQueryValidator : AbstractValidator<ListUserLedgerEntriesQuery>
    {
        public ListUserLedgerEntiresQueryValidator()
        {
            Include(new PagingParamasValidator<ListUserLedgerEntriesQuery>());
        }
    }

}
