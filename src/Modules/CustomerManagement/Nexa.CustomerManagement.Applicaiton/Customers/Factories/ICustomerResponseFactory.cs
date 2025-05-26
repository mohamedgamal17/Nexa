using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Applicaiton.Customers.Dtos;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nexa.CustomerManagement.Applicaiton.Customers.Factories
{
    public interface ICustomerResponseFactory : IResponseFactory<Customer, CustomerDto>
    {
    }
}
