using AutoMapper;
using Nexa.Accounting.Application.Wallets.Dtos;
using Nexa.Accounting.Application.Wallets.Services;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Wallets;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
namespace Nexa.Accounting.Application.Wallets.Commands.CreateWallet
{
    public class CreateWalletCommandHandler : IApplicationRequestHandler<CreateWalletCommand, WalletDto>
    {
        private readonly ISecurityContext _securityContext;
        private readonly IAccountingRepository<Wallet> _walletRepository;
        private readonly IWalletNumberGeneratorService _walletNumberGeneratorService;
        private readonly IMapper _mapper;
        public CreateWalletCommandHandler(ISecurityContext securityContext, IAccountingRepository<Wallet> walletRepository, IWalletNumberGeneratorService walletNumberGeneratorService, IMapper mapper)
        {
            _securityContext = securityContext;
            _walletRepository = walletRepository;
            _walletNumberGeneratorService = walletNumberGeneratorService;
            _mapper = mapper;
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

            return _mapper.Map<Wallet, WalletDto>(wallet);
        }
    }
}
