using Nexa.Accounting.Domain.FundingResources;
using Nexa.Accounting.Shared.Dtos;
using Nexa.BuildingBlocks.Application.Factories;

namespace Nexa.Accounting.Application.FundingResources.Factories
{
    public class BankAccountResponseFactory : ResponseFactory<BankAccount, BankAccountDto>, IBankAccountResponseFactory
    {
        public override Task<BankAccountDto> PrepareDto(BankAccount view)
        {
            var dto = new BankAccountDto
            {
                Id = view.Id,
                UserId = view.UserId,
                CustomerId = view.CustomerId,
                ProviderBankAccountId = view.ProviderBankAccountId,
                HolderName = view.HolderName,
                BankName = view.BankName,
                Country = view.Country,
                Currency = view.Currency,
                AccountNumberLast4 = view.AccountNumberLast4,
                RoutingNumber = view.RoutingNumber
            };

            return Task.FromResult(dto);
        }
    }
}
