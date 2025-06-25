using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Wallets.Services;
using Nexa.BuildingBlocks.Infrastructure.Modularity;

namespace Nexa.Accounting.Infrastructure.Installers
{
    public class ApplicationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly))
                .AddAutoMapper(Cfg => Cfg.AddMaps(Application.AssemblyReference.Assembly))
                .RegisterPoliciesHandlerFromAssembly(Application.AssemblyReference.Assembly)
                .RegisterFactoriesFromAssembly(Application.AssemblyReference.Assembly)
                .AddTransient<IWalletNumberGeneratorService, WalletNumberGeneratorService>();
        }
    }
}
