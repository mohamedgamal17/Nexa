using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.CustomerApplications;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.CustomerApplications
{
    public class CustomerApplicaitonTestFixture  : CustomerManagementTestFixture
    {
        protected ICustomerManagementRepository<CustomerApplication> CustomerApplicationRepository { get; }

        protected Faker Faker { get; }
        public CustomerApplicaitonTestFixture()
        {
            CustomerApplicationRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<CustomerApplication>>();
            Faker = new Faker();
        }

        protected async Task<Customer> CreateCustomerAsync(string? userId = null)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var customer = new Customer
                {
                    FirstName = Faker.Person.FirstName,
                    LastName = Faker.Person.LastName,
                    PhoneNumber = Faker.Person.Phone,
                    EmailAddress = Faker.Person.Email,
                    Gender = Faker.Person.Gender == Bogus.DataSets.Name.Gender.Male ? Gender.Male : Gender.Female,
                    BirthDate = Faker.Person.DateOfBirth,
                    UserId = userId ?? Guid.NewGuid().ToString()
                };
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                return await repository.InsertAsync(customer);
            });

        }

        protected async Task<CustomerApplication> CreateCustomerApplicationAsync(string customerId)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<CustomerApplication>>();

                var customerApplicaiton = new CustomerApplication
                {
                    FirstName = Faker.Person.FirstName,
                    LastName = Faker.Person.LastName,
                    PhoneNumber = Faker.Person.Phone,
                    EmailAddress = Faker.Person.Email,
                    Nationality = "US",
                    BirthDate = Faker.Person.DateOfBirth,
                    KycExternalId = Guid.NewGuid().ToString(),
                    Gender = Faker.Person.Gender == Bogus.DataSets.Name.Gender.Male ? Gender.Male : Gender.Female,
                    CustomerId = customerId,
                    Address = new Address
                    {
                        Country = "US",
                        City = Faker.Person.Address.City,
                        State = Faker.Person.Address.State,
                        StreetLine1 = Faker.Person.Address.Street,
                        StreetLine2 = Faker.Person.Address.Street,
                        PostalCode = Faker.Person.Address.ZipCode,
                        ZipCode = Faker.Person.Address.ZipCode
                    }
                };

                return await repository.InsertAsync(customerApplicaiton);
            });
        }

    }
}
