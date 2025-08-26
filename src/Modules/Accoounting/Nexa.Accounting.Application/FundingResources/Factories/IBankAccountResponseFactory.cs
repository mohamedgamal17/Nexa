using Nexa.Accounting.Application.FundingResources.Dtos;
using Nexa.Accounting.Domain.FundingResources;
using Nexa.BuildingBlocks.Application.Factories;

namespace Nexa.Accounting.Application.FundingResources.Factories
{
    public interface IBankAccountResponseFactory : IResponseFactory<BankAccount, BankAccountDto>
    {
    }
}
