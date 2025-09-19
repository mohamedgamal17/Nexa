using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Services;
using Nexa.Integrations.Baas.Abstractions.Services;
using Nexa.Transactions.Application.Transfers.Dtos;
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
}
