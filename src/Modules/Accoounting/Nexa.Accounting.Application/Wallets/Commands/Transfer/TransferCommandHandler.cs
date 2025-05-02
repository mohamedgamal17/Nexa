using AutoMapper;
using Nexa.Accounting.Application.Transactions.Dtos;
using Nexa.Accounting.Application.Transactions.Factories;
using Nexa.Accounting.Application.Transactions.Services;
using Nexa.Accounting.Application.Wallets.Policies;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Transactions;
using Nexa.Accounting.Domain.Wallets;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;

namespace Nexa.Accounting.Application.Wallets.Commands.Transfer
{
    public class TransferCommandHandler : IApplicationRequestHandler<TransferCommand, TransactionDto>
    {
        private readonly IWalletRepository _walletRepository;
        private readonly IApplicationAuthorizationService _applicationAuthorizationService;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionNumberGeneratorService _trasnctionNumberGeneratorService;
        private readonly ITransactionResponseFactory _transactionResponseFactory;

        public TransferCommandHandler(IWalletRepository walletRepository, IApplicationAuthorizationService applicationAuthorizationService, ITransactionRepository transctionRepository, ITransactionNumberGeneratorService trasnctionNumberGeneratorService, IMapper mapper, ITransactionResponseFactory transactionResponseFactory)
        {
            _walletRepository = walletRepository;
            _applicationAuthorizationService = applicationAuthorizationService;
            _transactionRepository = transctionRepository;
            _trasnctionNumberGeneratorService = trasnctionNumberGeneratorService;
            _transactionResponseFactory = transactionResponseFactory;
        }

        public async Task<Result<TransactionDto>> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var senderWallet = await _walletRepository.SingleAsync(x => x.Id == request.SenderId);

            var reciverWallet = await _walletRepository.SingleAsync(x => x.Id == request.ReciverId);

            var authorizationResult = await _applicationAuthorizationService.AuthorizeAsync(senderWallet, WalletOperationRequirment.IsOwner);

            if (authorizationResult.IsFailure)
            {
                return new Result<TransactionDto>(authorizationResult.Exception!);
            }

            if(senderWallet.Balance < request.Amount)
            {
                return new Result<TransactionDto>(new BusinessLogicException("Insufficient balance to complete the transfer."));
            }

            var transactionNumber = _trasnctionNumberGeneratorService.Generate();

            var transaction = new InternalTransaction(senderWallet.Id, transactionNumber, request.Amount, reciverWallet.Id);

            senderWallet.Depit(request.Amount);

            await _walletRepository.UpdateAsync(senderWallet);

            await _transactionRepository.InsertAsync(transaction);

            var view = await _transactionRepository.SinglVieweAsync(x => x.Id == transaction.Id);

            return await _transactionResponseFactory.PrepareDto(view);
        }
    }
}
