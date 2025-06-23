using Nexa.Accounting.Application.Transactions.Dtos;
using Nexa.Accounting.Application.Wallets.Factories;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Transactions;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Factories;

namespace Nexa.Accounting.Application.Transactions.Factories
{
    public class TransactionResponseFactory : ResponseFactory<TransactionView, TransactionDto>, ITransactionResponseFactory
    {

        private readonly IWalletResponseFactory _walletResponseFactory;

        public TransactionResponseFactory(IWalletResponseFactory walletResponseFactory)
        {
            _walletResponseFactory = walletResponseFactory;
        }

        public override async Task<List<TransactionDto>> PrepareListDto(List<TransactionView> views)
        {
            if(views.Count <= 0)
            {
                return new List<TransactionDto>();
            }
            var wallets = views
                .Where(x => x.Type == TransactionType.Internal)
                .Select(x => x.Reciver!)
                .ToList();

            wallets.Add(views.First().Wallet);

            var walletsDtos = await _walletResponseFactory.PrepareListDto(wallets);

            var walletsDictionary = walletsDtos.ToDictionary((x) => x.Id);

            var dtos = views.Select(transaction =>
                PrepareDto(transaction,
                    walletsDictionary[transaction.WalletId],
                    walletsDictionary!.GetValueOrDefault(transaction.ReciverId)
                )
              ).ToList();

            return dtos;
        }
        public override async Task<TransactionDto> PrepareDto(TransactionView view)
        {
            var wallets = new List<WalletView> { view.Wallet };

            if (view.Reciver != null) wallets.Add(view.Reciver);

            var walletsDto = await _walletResponseFactory.PrepareListDto(wallets);

            var wallet = walletsDto.First();

            var reciverWallet = view.Reciver != null ? walletsDto.Last() : null;

            return PrepareDto(view,wallet, reciverWallet);
        }

        private TransactionDto PrepareDto(TransactionView view, WalletDto wallet, WalletDto? reciverWallet = null)
        {
            var dto = new TransactionDto
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
