using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.CustomerManagement.Infrastructure;
namespace Nexa.CustomerManagement.Application.Tests
{
    public class CustomerManagementTestModuleInstaller : IModuleInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.InstallModule<CustomerManagementModuleInstaller>(configuration)
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

