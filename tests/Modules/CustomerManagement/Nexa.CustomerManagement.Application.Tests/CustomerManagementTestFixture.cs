using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Respawn.Graph;
using MassTransit.Testing;
namespace Nexa.CustomerManagement.Application.Tests
{
    [TestFixture]
    public class CustomerManagementTestFixture : TestFixture
    {
        protected ITestHarness TestHarness { get; private set; } = null!;
        protected override Task SetupAsync(IServiceCollection services, IConfiguration configuration)
        {
            Configuration["ConnectionStrings:Default"] = MsSqlServerContainerFixture.ConnectionString;
            services.InstallModule<CustomerManagementTestModuleInstaller>(configuration);
            return Task.CompletedTask;
        }
        protected override async Task InitializeAsync(IServiceProvider services)
        {
            TestHarness = services.GetTestHarness();
            await services.RunModulesBootstrapperAsync();
            await ResetSqlDb(services);

        }

        protected override async Task ShutdownAsync(IServiceProvider services)
        {
            await ResetSqlDb(services);
        }

        protected async Task ResetSqlDb(IServiceProvider services)
        {
            var config = services.GetRequiredService<IConfiguration>();

            var respwan = await Respawn.Respawner.CreateAsync(config.GetConnectionString("Default")!, new Respawn.RespawnerOptions
            {
                TablesToIgnore = new Table[]
                {
                  "sysdiagrams",
                  "tblUser",
                  "tblObjectType",
                  "__EFMigrationsHistory"
                },
                SchemasToInclude = new string[]
                {
                    "CustomerManagement"
                }

            });

            await respwan.ResetAsync(config.GetConnectionString("Default")!);
        }
    }
}
