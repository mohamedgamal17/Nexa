using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.Transactions.Infrastructure.EntityFramework;
namespace Nexa.Transactions.Infrastructure
{
    internal class TransactionsModuleBootStrapper : IModuleBootstrapper
    {
        public async Task Bootstrap(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<TransactionDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
