using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.FundingResources.Services;
using Nexa.Accounting.Application.Wallets.Services;
using Nexa.Accounting.Shared.Services;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using FluentValidation;
using Nexa.BuildingBlocks.Application.Extensions;
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
                .AddValidatorsFromAssembly(Application.AssemblyReference.Assembly)
                .AddTransient<IWalletNumberGeneratorService, WalletNumberGeneratorService>()
                .AddTransient<IWalletService, WalletService>()
                .AddTransient<IFundingResourceService, FundingResourceService>();
        }
    }
}
