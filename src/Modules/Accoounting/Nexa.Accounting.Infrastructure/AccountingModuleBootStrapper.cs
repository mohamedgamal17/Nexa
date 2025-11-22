using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Infrastructure.EntityFramework;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
namespace Nexa.Accounting.Infrastructure
{
    internal class AccountingModuleBootStrapper : IModuleBootstrapper
    {
        private static readonly SemaphoreSlim DbCreationSemaphore = new(1, 1);

        public async Task Bootstrap(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<AccountingDbContext>();

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
