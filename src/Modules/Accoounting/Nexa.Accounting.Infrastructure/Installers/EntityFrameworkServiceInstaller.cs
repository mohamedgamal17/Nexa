using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Infrastructure.EntityFramework;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Vogel.BuildingBlocks.EntityFramework.Interceptors;
using Microsoft.EntityFrameworkCore;
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
                   )
                .AddInterceptors(
                        sp.GetRequiredService<DispatchDomainEventInterceptor>()
                   );
            });

            services.AddTransient(typeof(IAccountingRepository<>), typeof(AccountingRepository<>));
        }
    }
}
