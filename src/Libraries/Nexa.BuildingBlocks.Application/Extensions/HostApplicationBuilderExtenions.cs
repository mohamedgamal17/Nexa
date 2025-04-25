using Microsoft.Extensions.Hosting;
using Nexa.BuildingBlocks.Application.Modularity;
namespace Nexa.BuildingBlocks.Application.Extensions
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
