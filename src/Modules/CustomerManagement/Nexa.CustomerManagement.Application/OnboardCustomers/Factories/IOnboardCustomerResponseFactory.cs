using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.OnboardCustomers.Factories
{
    public interface IOnboardCustomerResponseFactory : IResponseFactory<OnboardCustomer, OnboardCustomerDto>
    {
    }
}
