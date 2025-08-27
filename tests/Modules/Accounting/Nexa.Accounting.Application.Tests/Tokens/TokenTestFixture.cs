using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Tests.Fakers;

namespace Nexa.Accounting.Application.Tests.Tokens
{
    public abstract class TokenTestFixture : AccountingTestFixture
    {
        protected FakeCustomerService FakeCustomerService { get; }

        protected TokenTestFixture()
        {
            FakeCustomerService = ServiceProvider.GetRequiredService<FakeCustomerService>();
        }

    }
}
