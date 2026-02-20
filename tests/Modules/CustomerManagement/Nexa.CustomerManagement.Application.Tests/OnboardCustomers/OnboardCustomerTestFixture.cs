using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.OnboardCustomers
{
    public class OnboardCustomerTestFixture : CustomerManagementTestFixture
    {
        protected Faker Faker { get; }

        public OnboardCustomerTestFixture()
        {
            Faker = new Faker();
        }
        public Task<OnboardCustomer> CreateInitialOnboardCustomerAsync(string userId)
        {
            return WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<OnboardCustomer>>();

                var onboardCustomer = new OnboardCustomer(userId);

                return await repository.InsertAsync(onboardCustomer);
            });
        }


        public Task<OnboardCustomer> CreateCompletedOnboardCustomer(string userId)
        {
            return WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<OnboardCustomer>>();

                var onboardCustomer = new OnboardCustomer(userId);

                onboardCustomer.UpdatePhoneNumber(Faker.Person.Phone);

                onboardCustomer.UpdateEmailAddress(Faker.Person.Email);

                var address = Address.Create(
                        "US",
                        "San",
                        "CA",
                        "12 CA street",
                        "56454",
                        "4545"
                    );

                var customerInfo = CustomerInfo.Create(
                        Faker.Person.FirstName,
                        Faker.Person.LastName,
                        Faker.Person.DateOfBirth,
                        Gender.Male,
                        address
                    );

                onboardCustomer.UpdateCustomerInfo(customerInfo);

                onboardCustomer.MarkAsCompleted();

                return await repository.InsertAsync(onboardCustomer);
            });
        }
    }
}
