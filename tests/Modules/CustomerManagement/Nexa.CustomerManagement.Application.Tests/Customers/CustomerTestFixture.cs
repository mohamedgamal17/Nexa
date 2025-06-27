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

        protected async Task<Customer> CreateCustomerAsync(string? userId = null)
        {

            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var faker = new Faker();

                var customer = new Customer
                {
                    UserId = userId ?? Guid.NewGuid().ToString(),
                    FirstName = faker.Person.FirstName,
                    LastName = faker.Person.LastName,
                    Gender = faker.PickRandom<Gender>(),
                    EmailAddress = faker.Person.Email,
                    PhoneNumber = "13322767084",
                    BirthDate = DateTime.Now.AddYears(-25),                
                };


                return await repository.InsertAsync(customer);
            });
        }

    }
}
