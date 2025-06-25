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
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly))
                .RegisterPoliciesHandlerFromAssembly(AssemblyReference.Assembly)
                .RegisterFactoriesFromAssembly(AssemblyReference.Assembly)
                .AddTransient<ITransferNumberGeneratorService, TransferNumberGeneratorService>();
        }
    }
}
