using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Services;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.Transactions.Application.Extensions;
using Nexa.Transactions.Shared.Dtos;

namespace Nexa.Transactions.Application.Transfers.Queries.ListUserTransfers
{
    [Authorize]
    public class ListUserTransfersQuery : PagingParams , IQuery<Paging<TransferDto>> 
    {
        public string? WalletId { get; set; }
    }

    public class ListUserTransferQueryValidator : AbstractValidator<ListUserTransfersQuery>
    {
        private readonly IWalletService _walletService;
        public ListUserTransferQueryValidator(IWalletService walletService)
        {
            _walletService = walletService;

            Include(new PagingParamasValidator<ListUserTransfersQuery>());

            RuleFor(x => x.WalletId)
                .ValidateWallet(_walletService)
                .When(x => x.WalletId != null);
        }
    }
}
