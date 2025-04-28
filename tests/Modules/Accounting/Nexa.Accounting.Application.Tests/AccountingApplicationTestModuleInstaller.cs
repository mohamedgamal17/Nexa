using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexa.Accounting.Infrastructure;
using Nexa.Application.Tests;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
namespace Nexa.Accounting.Application.Tests
{
    public class AccountingApplicationTestModuleInstaller : IModuleInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.InstallModule<AccountingModuleInstaller>(configuration)
                .InstallModule<ApplicationTestModuleInstaller>(configuration);

            services.AddMassTransitTestHarness(busRegisterConfig =>
            {

                busRegisterConfig.AddConsumers(Application.AssemblyReference.Assembly);

                busRegisterConfig.UsingInMemory((context, inMemoryBusConfig) =>
                {
                    inMemoryBusConfig.ConfigureEndpoints(context);
                });
            });
        }
    }
}

