using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Nexa.Accounting.Infrastructure.EntityFramework;

namespace Nexa.Accounting.Application.Tests.EntityFramework
{
    internal class AccountingDbContextFactory : IDesignTimeDbContextFactory<AccountingDbContext>
    {
        public AccountingDbContext CreateDbContext(string[] args)
        {
            var config = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<AccountingDbContext>()
                .UseSqlServer(config.GetConnectionString("Default")!, (opt) =>
                {
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    opt.MigrationsAssembly(typeof(AccountingDbContext).Assembly.FullName);
                });


            return new AccountingDbContext(builder.Options);
        }

        private IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Nexa.Accounting.Application.Tests/"))
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}

