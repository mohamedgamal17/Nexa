using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.Accounting.Application.Wallets.Queries.GetUserLedgerEntryByIdQuery
{
    public class GetUserLedgerEntryByIdQuery : IQuery<LedgerEntryDto>
    {
        public string WalletId { get; set; }

        public string LedgerEntryId { get; set; }
    }
}
