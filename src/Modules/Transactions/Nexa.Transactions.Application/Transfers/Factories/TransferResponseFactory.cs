using Nexa.Accounting.Shared.Dtos;
using Nexa.Accounting.Shared.Services;
using Nexa.BuildingBlocks.Application.Factories;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Dtos;
namespace Nexa.Transactions.Application.Transfers.Factories
{
    public class TransferResponseFactory : ResponseFactory<TransferView, TransferDto>, ITransactionResponseFactory
    {
        private readonly IWalletService _walletService;

        private readonly IFundingResourceService _fundingResourceService;
        public TransferResponseFactory(IWalletService walletService, IFundingResourceService fundingResourceService)
        {
            _walletService = walletService;
            _fundingResourceService = fundingResourceService;
        }

        public override async Task<List<TransferDto>> PrepareListDto(List<TransferView> views)
        {
            if (views.Count <= 0)
            {
                return new List<TransferDto>();
            }

            var fundingResourceIds = views
                .Where(x => x.FundingResourceId != null)
                .Select(x => x.FundingResourceId!)
                .ToList();



            var walletIds = views.Select(x => x.WalletId).Union(
                views.Where(x=> x.ReciverId != null).Select(x => x.ReciverId!)
               ).ToList();

            List<WalletListDto> wallets = new List<WalletListDto>();

            List<BankAccountDto> fundingResources = new List<BankAccountDto>();

            if(fundingResourceIds.Count > 0)
            {
                fundingResources = await _fundingResourceService.ListByIds(fundingResourceIds);
            }

            if(walletIds.Count > 0)
            {
                wallets = await _walletService.ListWalletsByIds(walletIds);
            }


            var fundingResourceDictionary = fundingResources.ToDictionary(x => x.Id);

            var walletsDictionary = wallets.ToDictionary(x => x.Id);


            var dtos = views.Select((transaction) =>
            {
                var wallet = walletsDictionary[transaction.WalletId];

                var reciverWallet = transaction.ReciverId != null
                ? walletsDictionary[transaction.ReciverId]
                : default;

                var fundingResource = transaction.FundingResourceId != null
                 ? fundingResourceDictionary.GetValueOrDefault(transaction.FundingResourceId)
                 : default;

                return PrepareDto(transaction, wallet, reciverWallet , fundingResource);

            }).ToList();

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

        private TransferDto PrepareDto(TransferView view, WalletListDto wallet, WalletListDto? reciverWallet = null , BankAccountDto? bankAccountDto = null)
        {
            var dto = new TransferDto
            {
                Id = view.Id,
                UserId = view.UserId,
                WalletId = view.WalletId,
                Wallet = wallet,
                Number = view.Number,
                Amount = view.Amount,
                ReciverId = view.ReciverId,
                Reciver = reciverWallet,
                FundingResourceId = view.FundingResourceId,
                FundingResource = bankAccountDto,
                Direction = view.Direction,
                BankTransferType= view.BankTransferType,
                Status = view.Status,
                CompletedAt = view.CompletedAt,
                Type = view.Type
            };

            return dto;
        }
    }
}
