using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Respawn.Graph;
using MassTransit.Testing;
namespace Nexa.Transactions.Application.Tests
{
    public class TransactionsTestFixture : TestFixture
    {
        protected ITestHarness TestHarness { get; private set; } = null!;
        protected override Task SetupAsync(IServiceCollection services, IConfiguration configuration)
        {
            services.InstallModule<TransactionsTestModuleInstaller>(configuration);
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
                    "Transactions"
                }

            });

            await respwan.ResetAsync(config.GetConnectionString("Default")!);
        }
    }
}
