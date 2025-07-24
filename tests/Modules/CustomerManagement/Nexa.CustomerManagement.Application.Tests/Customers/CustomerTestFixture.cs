using Bogus;
using Bogus.Extensions.UnitedStates;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.Customers
{
    public class CustomerTestFixture : CustomerManagementTestFixture
    {
        protected Faker Faker { get; }

        protected IKYCProvider KycProvider { get; set; }
        public CustomerTestFixture()
        {
            Faker = new Faker();
            KycProvider = ServiceProvider.GetRequiredService<IKYCProvider>();
        }
        protected async Task<Customer> CreateCustomerWithoutInfo(string? userId = null )
        {
            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var customer = new Customer(userId ?? Guid.NewGuid().ToString(), Faker.Person.Phone, Faker.Person.Email);

                var client = await CreateKycClient(customer);

                customer.AddKycCustomerId(client.Id);

                return await repository.InsertAsync(customer);
            });
        }
        protected async Task<Customer> CreateCustomerAsync(string? userId = null,  VerificationState infoVerificationState = VerificationState.Pending)
        {

            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var customer = new Customer(userId ?? Guid.NewGuid().ToString(), Faker.Person.Phone, Faker.Person.Email, infoVerificationState);

                var address = Address.Create(
                        "US",
                        Faker.Address.City(),
                        Faker.Address.State(),
                        Faker.Address.StreetName(),
                        Faker.Person.Address.ZipCode,
                        Faker.Person.Address.ZipCode
                    );

                var info = CustomerInfo.Create(
                    Faker.Person.FirstName,
                    Faker.Person.LastName,
                    Faker.Person.DateOfBirth,
                    "US",
                    Faker.PickRandom<Gender>(),
                    Faker.Person.Ssn(),
                    address
                    );

                customer.UpdateInfo(info);

                var client = await CreateKycClient(customer);

                customer.AddKycCustomerId(client.Id);

                return await repository.InsertAsync(customer);
            });
        }
        protected async Task<KYCClient> CreateKycClient(Customer customer)
        {
            var request = new KYCClientRequest
            {
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,

            };


            if (customer.Info != null)
            {
                request.Info = new KYCClientInfo
                {
                    FirstName = customer.Info.FirstName,
                    LastName = customer.Info.LastName,
                    BirthDate = customer.Info.BirthDate,
                    Gender = customer.Info.Gender,
                    Nationality = customer.Info.Nationality,
                    SSN = customer.Info.IdNumber,
                    Address = customer.Info.Address
                };

            };

            var kycClient = await KycProvider.CreateClientAsync( request);

            return kycClient;
        }

    }
}
