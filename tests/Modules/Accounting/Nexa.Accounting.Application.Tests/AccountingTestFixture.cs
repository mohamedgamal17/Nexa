using Bogus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Wallets.Services;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Application.Tests;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Respawn.Graph;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Application.Tests.Fakers;
using MassTransit.Testing;
namespace Nexa.Accounting.Application.Tests
{

    public  class AccountingTestFixture : TestFixture
    {
        protected ITestHarness TestHarness { get; private set; } = null!;
        protected override Task SetupAsync(IServiceCollection services, IConfiguration configuration)
        {
            services.InstallModule<AccountingApplicationTestModuleInstaller>(configuration);
            return Task.CompletedTask;
        }
        protected override async Task InitializeAsync(IServiceProvider services)
        {
            TestHarness = services.GetTestHarness();
            await services.RunModulesBootstrapperAsync();
            await ResetSqlDb(services);
            await SeedData(services);
        }

      
        public async Task SeedData(IServiceProvider services)
        {
            var wallets =  await SeedWallets(services);

        }
     

        private async Task<List<Wallet>> SeedWallets(IServiceProvider services)
        {
            var walletRepository = services.GetRequiredService<IAccountingRepository<Wallet>>();

            var numberGeneratorServices = services.GetRequiredService<IWalletNumberGeneratorService>();

            var wallets = new WalletFaker(numberGeneratorService: numberGeneratorServices)
                .Generate(15);

            await walletRepository.InsertManyAsync(wallets);

            return wallets;
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
