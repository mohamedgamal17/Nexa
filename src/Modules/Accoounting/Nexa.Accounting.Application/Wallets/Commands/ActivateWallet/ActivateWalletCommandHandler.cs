using MediatR;
using Nexa.Accounting.Domain.Wallets;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.Integrations.Baas.Abstractions.Services;

namespace Nexa.Accounting.Application.Wallets.Commands.ActivateWallet
{
    public class ActivateWalletCommandHandler : IApplicationRequestHandler<ActivateWalletCommand, Unit>
    {
        private readonly IBaasWalletService _baasWalletService;
        private readonly IWalletRepository _walletRepository;

        public ActivateWalletCommandHandler(IBaasWalletService baasWalletService, IWalletRepository walletRepository)
        {
            _baasWalletService = baasWalletService;
            _walletRepository = walletRepository;
        }

        public async Task<Result<Unit>> Handle(ActivateWalletCommand request, CancellationToken cancellationToken)
        {
            var wallet = await _walletRepository
                .SingleAsync(x => x.CustomerId == request.CustomerId);

            string providerWalletId = wallet.ProviderWalletId;

            if(providerWalletId == null)
            {
                var baasWallet = await _baasWalletService.CreateWalletAsync(request.FintechId ,cancellationToken);

                providerWalletId = baasWallet.Id;
            }

            wallet.Activate(providerWalletId);

            await _walletRepository.UpdateAsync(wallet);

            return Unit.Value;
        }
    }
}
