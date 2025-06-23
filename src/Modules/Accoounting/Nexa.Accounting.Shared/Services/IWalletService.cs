using Nexa.Accounting.Shared.Dtos;
namespace Nexa.Accounting.Shared.Services
{
    public interface IWalletService
    {
        Task<List<WalletListDto>> ListWalletsByIds(List<string> walletIds, CancellationToken cancellationToken = default); 
        Task<WalletDto> GetWalletById(string walletId, CancellationToken cancellationToken = default);
    }
}
