using Bogus;
using Bogus.Extensions.Canada;
using Bogus.Extensions.Portugal;
using Bogus.Extensions.UnitedStates;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.Customers
{
    public class CustomerTestFixture : CustomerManagementTestFixture
    {
        protected Faker Faker { get; }

        public CustomerTestFixture()
        {
            Faker = new Faker();
        }

        protected async Task<Customer> CreateCustomerAsync(string? userId = null)
        {

            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();


                var customer = new Customer(userId ?? Guid.NewGuid().ToString(), Faker.Person.Phone, Faker.Person.Email);


                return await repository.InsertAsync(customer);
            });
        }

    }
}
