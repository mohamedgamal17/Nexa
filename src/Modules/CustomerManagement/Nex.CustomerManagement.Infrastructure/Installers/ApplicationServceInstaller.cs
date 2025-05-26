using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
namespace Nexa.CustomerManagement.Infrastructure.Installers
{
    public class ApplicationServceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly))
                .RegisterPoliciesHandlerFromAssembly(Application.AssemblyReference.Assembly)
                .RegisterFactoriesFromAssembly(Application.AssemblyReference.Assembly);    
        }
    }
}
