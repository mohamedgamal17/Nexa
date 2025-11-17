using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Consts;
using Nexa.Accounting.Shared.Services;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.Transactions.Application.Extensions;
using Nexa.Transactions.Application.Transfers.Dtos;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Consts;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Application.Transfers.Commands.CreateBankTransfer
{
    [Authorize]
    public class CreateBankTransferCommand : ICommand<TransferDto>
    {
        public string WalletId { get; set; }
        public string FundingResourceId { get; set; }
        public decimal Amount { get; set; }
        public TransferDirection Direction { get; set; }
    }


    public class CreateBankTransferCommandValidator : AbstractValidator<CreateBankTransferCommand>
    {
        private readonly IWalletService _walletService;
        private readonly IFundingResourceService _fundingResourceService;
        public CreateBankTransferCommandValidator(IWalletService walletService, IFundingResourceService fundingResourceService)
        {
            _walletService = walletService;
            _fundingResourceService = fundingResourceService;

            RuleFor(x => x.WalletId)
                .ValidateWallet(_walletService);

            RuleFor(x => x.FundingResourceId)
                .ValidateFundingResoruce(_fundingResourceService);

            RuleFor(x => x.Amount)
                .ValidateMoney();
        }
    }
}
