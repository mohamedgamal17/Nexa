using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Shared.Services;
using Nexa.Application.Tests;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.Transactions.Application.Tests.Fakers;
using Nexa.Transactions.Infrastructure;
namespace Nexa.Transactions.Application.Tests
{
    public class TransactionsTestModuleInstaller : IModuleInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.InstallModule<TransactionsModuleInstaller>(configuration)
                .InstallModule<ApplicationTestModuleInstaller>(configuration);

            services.AddMassTransitTestHarness(busRegisterConfig =>
            {

                busRegisterConfig.AddConsumers(AssemblyReference.Assembly);

                busRegisterConfig.UsingInMemory((context, inMemoryBusConfig) =>
                {
                    inMemoryBusConfig.ConfigureEndpoints(context);
                });
            });


            services.AddTransient<IWalletService, FakeWalletService>();
            services.AddTransient<IFundingResourceService, FakeFundingResourceService>();

            services.AddTransient<FakeWalletService>();
            services.AddTransient<FakeFundingResourceService>();

        }
    }
}

