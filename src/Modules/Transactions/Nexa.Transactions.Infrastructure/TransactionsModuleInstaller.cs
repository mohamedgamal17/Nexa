using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using System.Reflection;
namespace Nexa.Transactions.Infrastructure
{
    public class TransactionsModuleInstaller : IModuleInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.InstallServiceFromAssembly(configuration,
             Assembly.GetExecutingAssembly()
           );

            services.AddTransient<IModuleBootstrapper, TransactionsModuleBootStrapper>();
        }
    }
}
