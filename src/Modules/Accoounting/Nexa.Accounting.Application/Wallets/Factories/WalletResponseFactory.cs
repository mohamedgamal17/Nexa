using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Factories;
using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Services;

namespace Nexa.Accounting.Application.Wallets.Factories
{
    public class WalletResponseFactory : ResponseFactory<WalletView, WalletDto>, IWalletResponseFactory
    {
        private readonly ICustomerService _customerService;

        public WalletResponseFactory(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public async Task<Paging<WalletListDto>> PreparePagingWalletListDto(Paging<WalletView> paged)
        {
            var data = await PrepareWalletListDto(paged.Data.ToList());

            var reuslt = new Paging<WalletListDto>() { Data = data, Info = paged.Info };

            return reuslt;
        }
        public override async Task<List<WalletDto>> PrepareListDto(List<WalletView> views)
        {
            var customerIds = views.Select(x => x.CustomerId).Distinct().ToList();

            var customers = await _customerService.ListPublicByIds(customerIds);

            var customerDictionary = customers.ToDictionary((v) => v.Id);

            var results = views.Select(x => PrepareDto(x, customerDictionary.GetValueOrDefault(x.CustomerId))).ToList();

            return results;
        }

        public async Task<List<WalletListDto>> PrepareWalletListDto(List<WalletView> wallets)
        {

            var customerIds = wallets.Select(x => x.CustomerId).Distinct().ToList();

            var customers = await _customerService.ListPublicByIds(customerIds);

            var customerDictionary = customers.ToDictionary((v) => v.Id);

            var result = wallets.Select(x=> PrepareWalletListDto(x, customerDictionary.GetValueOrDefault(x.CustomerId))).ToList();

            return result;
        }

        public async Task<WalletListDto> PrepareWalletListDto(WalletView wallet )
        {
            var customer = await _customerService.GetPublicById(wallet.CustomerId);

            return PrepareWalletListDto(wallet, customer);
        }

        private WalletListDto PrepareWalletListDto(WalletView wallet, CustomerPublicDto? customer)
        {
            var dto = new WalletListDto
            {
                Id = wallet.Id,
                Number = wallet.Number,
                UserId = wallet.UserId,
                CustomerId = wallet.CustomerId,
                Customer = customer
            };

            return dto;
        }

        public override async Task<WalletDto> PrepareDto(WalletView view)
        {
            var customer = await _customerService.GetPublicById(view.CustomerId);

            return PrepareDto(view,customer);
        }


        private WalletDto PrepareDto(WalletView wallet, CustomerPublicDto? customer)
        {
            var dto = new WalletDto
            {
                Id = wallet.Id,
                ProviderWalletId = wallet.ProviderWalletId,
                UserId = wallet.UserId,
                CustomerId = wallet.CustomerId,
                Number = wallet.Number,
                Balance = wallet.Balance,
                Customer = customer
            };

            return dto;
        }

    }
}
