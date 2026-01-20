using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Shared.Consts;
using Nexa.Accounting.Shared.Services;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.Transactions.Application.Extensions;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Dtos;

namespace Nexa.Transactions.Application.Transfers.Commands.CreateNetworkTransfer
{
    [Authorize]
    public class CreateNetworkTransferCommand : ICommand<TransferDto>
    {
        public string SenderId { get; set; }
        public string ReciverId { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateNetworkTransferCommandValidator : AbstractValidator<CreateNetworkTransferCommand>
    {
        private readonly IWalletService _walletService;
        public CreateNetworkTransferCommandValidator(IWalletService walletService)
        {
            _walletService = walletService;

            RuleFor(x => x.SenderId)
               .ValidateWallet(_walletService);

            RuleFor(x => x.ReciverId)
               .ValidateWallet(_walletService);

            RuleFor(x => x.Amount)
                .ValidateMoney();
        }
    }
}
