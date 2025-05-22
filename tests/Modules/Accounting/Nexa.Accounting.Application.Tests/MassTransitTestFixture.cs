using MassTransit.Testing;

namespace Nexa.Accounting.Application.Tests
{
    [TestFixture]
    public class MassTransitTestFixture : AccountingTestFixture
    {
        protected override async Task InitializeAsync(IServiceProvider services)
        {
            await base.InitializeAsync(services);

            await TestHarness.Start();
        }

        protected override async Task ShutdownAsync(IServiceProvider services)
        {
            await base.ShutdownAsync(services);

            await TestHarness.Stop();
        }
    }
}
