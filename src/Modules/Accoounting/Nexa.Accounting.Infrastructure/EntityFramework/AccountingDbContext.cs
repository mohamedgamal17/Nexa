using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Vogel.BuildingBlocks.EntityFramework;
namespace Nexa.Accounting.Infrastructure.EntityFramework
{
    public class AccountingDbContext : NexaDbContext<AccountingDbContext>
    {
        public AccountingDbContext(DbContextOptions<AccountingDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(AccountingDbConstants.Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
