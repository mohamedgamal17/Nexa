using Nexa.Accounting.Shared.Dtos;
using Nexa.Accounting.Shared.Services;
using Nexa.BuildingBlocks.Application.Factories;
using Nexa.Transactions.Application.Transfers.Dtos;
using Nexa.Transactions.Domain.Transfers;
namespace Nexa.Transactions.Application.Transfers.Factories
{
    public class TransferResponseFactory : ResponseFactory<TransferView, TransferDto>, ITransactionResponseFactory
    {
        private readonly IWalletService _walletService;

        public TransferResponseFactory(IWalletService walletService)
        {
            _walletService = walletService;
        }

        public override async Task<List<TransferDto>> PrepareListDto(List<TransferView> views)
        {
            if (views.Count <= 0)
            {
                return new List<TransferDto>();
            }

            var walletIds = views.Select(x => x.WalletId).Union(
                views.Where(x=> x.ReciverId != null).Select(x => x.ReciverId!)
               ).ToList();

            var wallets = await _walletService.ListWalletsByIds(walletIds);

            var walletsDictionary = wallets.ToDictionary(x => x.Id);


            var dtos = views.Select(transaction =>
                PrepareDto(transaction,
                    walletsDictionary[transaction.WalletId],
                    walletsDictionary!.GetValueOrDefault(transaction.ReciverId)
                )
              ).ToList();

            return dtos;
        }
        public override async Task<TransferDto> PrepareDto(TransferView view)
        {
            var walletsIds = new List<string> { view.WalletId };

            if (view.ReciverId != null) walletsIds.Add(view.ReciverId);

            var walletsDto = await _walletService.ListWalletsByIds(walletsIds);

            var wallet = walletsDto.First();

            var reciverWallet = view.ReciverId != null ? walletsDto.Last() : null;

            return PrepareDto(view, wallet, reciverWallet);
        }

        private TransferDto PrepareDto(TransferView view, WalletListDto wallet, WalletListDto? reciverWallet = null)
        {
            var dto = new TransferDto
            {
                Id = view.Id,
                WalletId = view.WalletId,
                Wallet = wallet,
                Number = view.Number,
                Amount = view.Amount,
                ReciverId = view.ReciverId,
                Reciver = reciverWallet,
                PaymentId = view.PaymentId,
                Direction = view.Direction,
                Status = view.Status,
                CompletedAt = view.CompletedAt,
                Type = view.Type
            };

            return dto;
        }
    }
}
