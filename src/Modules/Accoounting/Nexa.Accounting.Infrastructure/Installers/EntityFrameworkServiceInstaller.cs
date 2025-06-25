using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Infrastructure.EntityFramework;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Microsoft.EntityFrameworkCore;
using Nexa.Accounting.Infrastructure.EntityFramework.Repositories;
using Nexa.Accounting.Domain.Wallets;
namespace Nexa.Accounting.Infrastructure.Installers
{
    public class EntityFrameworkServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AccountingDbContext>((sp, opt) =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"), cfg => cfg
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                   );
                
            });

            services.AddTransient(typeof(IAccountingRepository<>), typeof(AccountingRepository<>))
                .AddTransient<IWalletRepository, WalletRepository>();
        }
    }
}
