using Nexa.Accounting.Shared.Dtos;
using Nexa.Accounting.Shared.Services;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Transactions.Application.Transfers.Dtos;
using Nexa.Transactions.Application.Transfers.Factories;
using Nexa.Transactions.Application.Transfers.Services;
using Nexa.Transactions.Domain.Transfers;

namespace Nexa.Transactions.Application.Transfers.Commands.CreateNetworkTransfer
{
    public class CreateNetworkTransferCommandHandler : IApplicationRequestHandler<CreateNetworkTransferCommand, TransferDto>
    {
        private readonly IWalletService _walletService;
        private readonly ITransferRepository _transferRepository;
        private readonly ITransferNumberGeneratorService _transferNumberGeneratorService;
        private readonly ITransactionResponseFactory _transferResponseFactory;
        private readonly ISecurityContext _securityContext;

        public CreateNetworkTransferCommandHandler(IWalletService walletService, ITransferRepository transferRepository, ITransferNumberGeneratorService transferNumberGeneratorService, ITransactionResponseFactory transferResponseFactory, ISecurityContext securityContext)
        {
            _walletService = walletService;
            _transferRepository = transferRepository;
            _transferNumberGeneratorService = transferNumberGeneratorService;
            _transferResponseFactory = transferResponseFactory;
            _securityContext = securityContext;
        }

        public async Task<Result<TransferDto>> Handle(CreateNetworkTransferCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;  

            var senderWallet = await _walletService.GetWalletById(request.SenderId);

            var reciverWallet = await _walletService.GetWalletById(request.ReciverId);

            bool isWalletOwner = IsWalletOwner(senderWallet, userId);

            if (!isWalletOwner)
            {
                return new Result<TransferDto>(new ForbiddenAccessException());
            }

            if (senderWallet.Balance < request.Amount)
            {
                return new Result<TransferDto>(new BusinessLogicException("Insufficient balance to complete the transfer."));
            }

            var transferNumber = _transferNumberGeneratorService.Generate();


            var transfer = new NetworkTransfer(userId,senderWallet.Id, reciverWallet.Id, transferNumber, request.Amount);


            await _transferRepository.InsertAsync(transfer);


            var response = await _transferRepository.GetViewByIdAsync( transfer.Id);

            return await _transferResponseFactory.PrepareDto(response);
        }


        private bool IsWalletOwner(WalletDto wallet  , string userId)
        {
            return wallet.UserId == userId;
        }
    }
}
