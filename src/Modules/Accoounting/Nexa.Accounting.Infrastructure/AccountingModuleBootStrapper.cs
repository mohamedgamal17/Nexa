using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Infrastructure.EntityFramework;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
namespace Nexa.Accounting.Infrastructure
{
    internal class AccountingModuleBootStrapper : IModuleBootstrapper
    {
        public async Task Bootstrap(IServiceProvider serviceProvider)
        {
            var dbContext = serviceProvider.GetRequiredService<AccountingDbContext>();

            await dbContext.Database.MigrateAsync();
        }
    }
}
