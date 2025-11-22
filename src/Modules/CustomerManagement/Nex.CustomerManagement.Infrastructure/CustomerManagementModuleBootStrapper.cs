using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.CustomerManagement.Infrastructure.EntityFramework;

namespace Nexa.CustomerManagement.Infrastructure
{
    public class CustomerManagementModuleBootStrapper : IModuleBootstrapper
    {
        private static readonly SemaphoreSlim DbCreationSemaphore = new(1, 1);

        public async Task Bootstrap(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<CustomerManagementDbContext>();

            await DbCreationSemaphore.WaitAsync();

            try
            {
                await dbContext.Database.MigrateAsync();

            }
            finally
            {
                DbCreationSemaphore.Release();
            }

        }
    }
}
