using Nexa.Accounting.Shared.Consts;
using Nexa.Accounting.Shared.Dtos;
using Nexa.Accounting.Shared.Enums;
using Nexa.Accounting.Shared.Services;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Transactions.Application.Transfers.Factories;
using Nexa.Transactions.Application.Transfers.Services;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Dtos;
namespace Nexa.Transactions.Application.Transfers.Commands.CreateBankTransfer
{
    public class CreateBankTransferCommandHandler : IApplicationRequestHandler<CreateBankTransferCommand, TransferDto>
    {
        private readonly IWalletService _walletService;
        private readonly IFundingResourceService _fundingResourceService;
        private readonly ITransferRepository _transferRepository;
        private readonly ITransferNumberGeneratorService _transferNumberGenerator;
        private readonly ITransactionResponseFactory _transactionResponseFactory;
        private readonly ISecurityContext _securityContext;

        public CreateBankTransferCommandHandler(IWalletService walletService, IFundingResourceService fundingResourceService, ITransferRepository transferRepository, ITransferNumberGeneratorService transferNumberGenerator, ITransactionResponseFactory transactionResponseFactory, ISecurityContext securityContext)
        {
            _walletService = walletService;
            _fundingResourceService = fundingResourceService;
            _transferRepository = transferRepository;
            _transferNumberGenerator = transferNumberGenerator;
            _transactionResponseFactory = transactionResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<TransferDto>> Handle(CreateBankTransferCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id!;

            var wallet = (await _walletService.GetWalletById(request.WalletId))!;

            if(!IsWalletOwner(wallet, userId))
            {
                return new ForbiddenAccessException(WalletErrorConsts.WalletNotOwned);
            }

            var fundingResource = (await _fundingResourceService.GetFundingResourceById(request.FundingResourceId))!;

            if (!IsFundingResourceOwner(fundingResource, userId))
            {
                return new ForbiddenAccessException(BankAccountErrorConsts.BankAccountNotOwned);
            }

            string transferNumber = _transferNumberGenerator.Generate();

            BankTransfer transfer;

            if(request.Direction == Shared.Enums.TransferDirection.Credit)
            {
                transfer = BankTransfer.Deposit(userId, request.WalletId, transferNumber, request.Amount, request.FundingResourceId);
            }
            else
            {
                transfer = BankTransfer.Withdraw(userId, request.WalletId, transferNumber, request.Amount, request.FundingResourceId);
            }

             
            await _transferRepository.InsertAsync(transfer);


            var view = await _transferRepository.GetViewByIdAsync(transfer.Id);

            return await _transactionResponseFactory.PrepareDto(view);

        }

        private bool IsWalletOwner(WalletDto wallet, string userId) => wallet.UserId == userId;

        private bool IsFundingResourceOwner(BankAccountDto bankAccountDto, string userId) => bankAccountDto.UserId == userId;
    }
}
