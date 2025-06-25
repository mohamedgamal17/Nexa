using Nexa.Accounting.Shared.Dtos;
using Nexa.Accounting.Shared.Services;

namespace Nexa.Transactions.Application.Tests.Fakers
{
    public class FakeWalletService : IWalletService
    {
        private static List<WalletDto> _inMemoryWalletDb = new();

        public Task<WalletDto> GetWalletById(string walletId, CancellationToken cancellationToken = default)
        {
            var wallet = _inMemoryWalletDb.Single(x => x.Id == walletId);

            return Task.FromResult(wallet);
        }

        public  Task<List<WalletListDto>> ListWalletsByIds(List<string> walletIds, CancellationToken cancellationToken = default)
        {
            var wallets = _inMemoryWalletDb.Where(x => walletIds.Contains(x.Id))
                .Select(x => new WalletListDto
                {
                    Id = x.Id,
                    Number = x.Number,
                    UserId = x.UserId
                }).ToList();

            return Task.FromResult(wallets);
        }


        public Task<WalletDto> AddWalletAsync(WalletDto wallet , CancellationToken cancellationToken = default)
        {
            _inMemoryWalletDb.Add(wallet);

            return Task.FromResult(wallet);
        }
    }
}
