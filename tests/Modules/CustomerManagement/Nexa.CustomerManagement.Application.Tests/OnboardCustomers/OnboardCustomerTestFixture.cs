using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.OnboardCustomers;

namespace Nexa.CustomerManagement.Application.Tests.OnboardCustomers
{
    public class OnboardCustomerTestFixture : CustomerManagementTestFixture
    {

        public Task<OnboardCustomer> CreateInitialOnboardCustomerAsync(string userId)
        {
            return WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<OnboardCustomer>>();

                var onboardCustomer = new OnboardCustomer(userId);

                return await repository.InsertAsync(onboardCustomer);
            });
        }
    }
}
