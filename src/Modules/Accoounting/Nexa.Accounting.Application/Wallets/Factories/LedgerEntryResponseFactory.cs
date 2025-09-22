using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Factories;

namespace Nexa.Accounting.Application.Wallets.Factories
{
    public class LedgerEntryResponseFactory : ResponseFactory<LedgerEntry, LedgerEntryDto>, ILedgerEntryResponseFactory
    {
        public override Task<LedgerEntryDto> PrepareDto(LedgerEntry view)
        {
            var dto = new LedgerEntryDto
            {
                Id = view.Id,
                WalletId = view.WalletId,
                Amount = view.Amount,
                TransactionId = view.TransactionId,
                Type = view.Type,
                Direction = view.Direction
            };

            return Task.FromResult(dto);
        }
    }
}
