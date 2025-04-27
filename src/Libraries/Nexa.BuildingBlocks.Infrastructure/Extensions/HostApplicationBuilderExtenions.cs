using Microsoft.Extensions.Hosting;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
namespace Nexa.BuildingBlocks.Infrastructure.Extensions
{
    public static class HostApplicationBuilderExtenions
    {
        public static IHostApplicationBuilder InstallModule<T>(this IHostApplicationBuilder host) where
            T : class, IModuleInstaller
        {
            var moduleType = typeof(T);

            var module = ((IModuleInstaller)Activator.CreateInstance(moduleType)!)!;

            module.Install(host.Services, host.Configuration, host.Environment);

            return host;
        }
    }
}
