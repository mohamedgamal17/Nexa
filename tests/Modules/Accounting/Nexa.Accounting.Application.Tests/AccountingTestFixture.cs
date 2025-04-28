using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexa.Accounting.Infrastructure.EntityFramework;
using Nexa.Application.Tests;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Respawn.Graph;

namespace Nexa.Accounting.Application.Tests
{
    [TestFixture]
    public class AccountingTestFixture : TestFixture
    {

        [Test]
        public void Test()
        {
            var db = ServiceProvider.GetRequiredService<AccountingDbContext>();

            Console.WriteLine();
        }
        protected override Task SetupAsync(IServiceCollection services, IConfiguration configuration)
        {
            services.InstallModule<AccountingApplicationTestModuleInstaller>(configuration);
            return Task.CompletedTask;
        }
        protected override async Task InitializeAsync(IServiceProvider services)
        {
            //await ResetSqlDb(services);
            await services.RunModulesBootstrapperAsync();
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
                    "Accounting"
                }

            });

            await respwan.ResetAsync(config.GetConnectionString("Default")!);
        }
    }
}
