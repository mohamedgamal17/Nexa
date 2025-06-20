using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Factories
{
    public interface ICustomerResponseFactory : IResponseFactory<Customer, CustomerDto>
    {
    }
}
