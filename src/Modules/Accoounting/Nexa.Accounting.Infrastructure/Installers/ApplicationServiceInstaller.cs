using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexa.Accounting.Application.Transactions.Services;
using Nexa.Accounting.Application.Wallets.Services;
using Nexa.BuildingBlocks.Infrastructure.Modularity;

namespace Nexa.Accounting.Infrastructure.Installers
{
    public class ApplicationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services
                .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Application.AssemblyReference.Assembly))
                .AddAutoMapper(Cfg => Cfg.AddMaps(Application.AssemblyReference.Assembly))
                .RegisterPoliciesHandlerFromAssembly(Application.AssemblyReference.Assembly)
                .AddTransient<IWalletNumberGeneratorService, WalletNumberGeneratorService>()
                .AddTransient<ITransactionNumberGeneratorService, TransactionNumberGeneratorService>();
        }
    }
}
