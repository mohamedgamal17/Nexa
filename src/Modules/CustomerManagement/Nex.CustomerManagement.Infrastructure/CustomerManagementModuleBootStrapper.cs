using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.CustomerManagement.Infrastructure.EntityFramework;

namespace Nexa.CustomerManagement.Infrastructure
{
    public class CustomerManagementModuleBootStrapper : IModuleBootstrapper
    {
        public async Task Bootstrap(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<CustomerManagementDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
