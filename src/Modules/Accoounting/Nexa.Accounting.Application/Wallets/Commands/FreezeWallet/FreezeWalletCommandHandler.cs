using MediatR;
using Nexa.Accounting.Domain.Wallets;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;

namespace Nexa.Accounting.Application.Wallets.Commands.FreezeWallet
{
    public class FreezeWalletCommandHandler : IApplicationRequestHandler<FreezeWalletCommand, Unit>
    {
        private readonly IWalletRepository _walletRepository;

        public FreezeWalletCommandHandler(IWalletRepository walletRepository)
        {
            _walletRepository = walletRepository;
        }

        public async Task<Result<Unit>> Handle(FreezeWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository
              .SingleAsync(x => x.CustomerId == request.CustomerId);

            wallet.Freeze();

            await _walletRepository.UpdateAsync(wallet);

            return Unit.Value;
        }
    }
}
