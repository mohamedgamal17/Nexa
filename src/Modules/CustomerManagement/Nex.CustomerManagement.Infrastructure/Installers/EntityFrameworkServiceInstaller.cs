using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Microsoft.EntityFrameworkCore;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Infrastructure.EntityFramework;
using Nexa.CustomerManagement.Infrastructure.EntityFramework.Repositories;
namespace Nexa.CustomerManagement.Infrastructure.Installers
{
    public class EntityFrameworkServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<CustomerManagementDbContext>((sp, opt) =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"), cfg => cfg
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                   );

            });

            services.AddTransient(typeof(ICustomerManagementRepository<>), typeof(CustomerManagementRepository<>));
        }
    }
}
