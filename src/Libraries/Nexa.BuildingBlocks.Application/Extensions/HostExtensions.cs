using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexa.BuildingBlocks.Application.Modularity;
namespace Nexa.BuildingBlocks.Application.Extensions
{
    public static class HostExtensions
    {
        public static async Task RunModulesBootstrapperAsync(this IHost host)
        {
            await host.Services.RunModulesBootstrapperAsync();
        }

        public static async Task RunModulesBootstrapperAsync(this IServiceProvider serviceProvider)
        {
            var moduleBootstrappers = serviceProvider.GetServices<IModuleBootstrapper>();
            foreach (var moduleBootstrap in moduleBootstrappers)
            {
                await moduleBootstrap.Bootstrap(serviceProvider);
            }
        }
    }
}
