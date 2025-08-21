using AutoMapper;
using Nexa.Accounting.Application.Wallets.Factories;
using Nexa.Accounting.Application.Wallets.Services;
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
        private readonly IWalletRepository _walletRepository;
        private readonly IWalletNumberGeneratorService _walletNumberGeneratorService;
        private readonly IWalletResponseFactory _walletResponseFactory;

        public CreateWalletCommandHandler(IWalletRepository walletRepository, IWalletNumberGeneratorService walletNumberGeneratorService, IWalletResponseFactory walletResponseFactory)
        {
            _walletRepository = walletRepository;
            _walletNumberGeneratorService = walletNumberGeneratorService;
            _walletResponseFactory = walletResponseFactory;
        }

        public async Task<Result<WalletDto>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
        {
            string walletNumber = _walletNumberGeneratorService.Generate();

            var wallet = new Wallet(request.CustomerId, request.UserId,walletNumber);

            await _walletRepository.InsertAsync(wallet);

            var view = await _walletRepository.SinglVieweAsync(x => x.Id == wallet.Id);

            return await _walletResponseFactory.PrepareDto(view);
        }
    }
}
