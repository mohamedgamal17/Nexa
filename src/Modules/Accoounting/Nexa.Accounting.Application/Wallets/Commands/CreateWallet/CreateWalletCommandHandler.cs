using AutoMapper;
using Nexa.Accounting.Application.Wallets.Factories;
using Nexa.Accounting.Application.Wallets.Services;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
namespace Nexa.Accounting.Application.Wallets.Commands.CreateWallet
{
    public class CreateWalletCommandHandler : IApplicationRequestHandler<CreateWalletCommand, WalletDto>
    {
        private readonly ISecurityContext _securityContext;
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletNumberGeneratorService _walletNumberGeneratorService;
        private readonly IWalletResponseFactory _walletResponseFactory;

        public CreateWalletCommandHandler(ISecurityContext securityContext, IWalletRepository walletRepository, IWalletNumberGeneratorService walletNumberGeneratorService, IWalletResponseFactory walletResponseFactory)
        {
            _securityContext = securityContext;
            _walletRepository = walletRepository;
            _walletNumberGeneratorService = walletNumberGeneratorService;
            _walletResponseFactory = walletResponseFactory;
        }

        public async Task<Result<WalletDto>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            bool hasWallet = await _walletRepository.AnyAsync(x => x.UserId == userId);

            if (hasWallet)
            {
                return new Result<WalletDto>(new BusinessLogicException("Current user has already wallet"));
            }

            string walletNumber = _walletNumberGeneratorService.Generate();

            var wallet = new Wallet(walletNumber, userId);

            await _walletRepository.InsertAsync(wallet);

            var view = await _walletRepository.SinglVieweAsync(x => x.Id == wallet.Id);

            return await _walletResponseFactory.PrepareDto(view);
        }
    }
}
