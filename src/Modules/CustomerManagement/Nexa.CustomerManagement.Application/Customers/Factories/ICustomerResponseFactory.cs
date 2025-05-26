using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Application.Customers.Dtos;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nexa.CustomerManagement.Application.Customers.Factories
{
    public interface ICustomerResponseFactory : IResponseFactory<Customer, CustomerDto>
    {
    }
}
