using Microsoft.EntityFrameworkCore;
using Nexa.Accounting.Application.Wallets.Factories;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.Accounting.Shared.Services;

namespace Nexa.Accounting.Application.Wallets.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletResponseFactory _walletResponseFactory;
        private readonly IWalletRepository _walletRepository;
        public WalletService(IWalletResponseFactory walletResponseFactory, IWalletRepository walletRepository)
        {
            _walletResponseFactory = walletResponseFactory;
            _walletRepository = walletRepository;
        }

        public async Task<WalletDto> GetWalletById(string walletId, CancellationToken cancellationToken = default)
        {
            var wallet = await _walletRepository.SinglVieweAsync(x => x.Id == walletId);

            return await _walletResponseFactory.PrepareDto(wallet);
        }
        
        public async Task<List<WalletListDto>> ListWalletsByIds(List<string> walletIds, CancellationToken cancellationToken = default)
        {
            var wallets = await _walletRepository
                .QueryView().Where(x => walletIds.Contains(x.Id)).ToListAsync();

            return await _walletResponseFactory.PrepareWalletListDto(wallets);
        }
    }
}
