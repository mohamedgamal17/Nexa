using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.Transactions.Infrastructure.EntityFramework;
namespace Nexa.Transactions.Infrastructure
{
    internal class TransactionsModuleBootStrapper : IModuleBootstrapper
    {
        private static readonly SemaphoreSlim DbCreationSemaphore = new(1, 1);

        public async Task Bootstrap(IServiceProvider serviceProvider)
        {     
            var dbContext = serviceProvider.GetRequiredService<TransactionDbContext>();

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
