using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.Transactions.Application.Transfers.Services;
namespace Nexa.Transactions.Infrastructure.Installers
{
    public class ApplicationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly))
                .RegisterPoliciesHandlerFromAssembly(Application.AssemblyReference.Assembly)
                .RegisterFactoriesFromAssembly(Application.AssemblyReference.Assembly)
                .AddTransient<ITransferNumberGeneratorService, TransferNumberGeneratorService>();
        }
    }
}
