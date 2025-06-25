using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Transactions.Infrastructure.EntityFramework;
namespace Nexa.Transactions.Application.Tests.EntityFramework
{
    public class TransactionDbContextDbContextFactory : IDesignTimeDbContextFactory<TransactionDbContext>
    {
        public TransactionDbContext CreateDbContext(string[] args)
        {
            var config = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<TransactionDbContext>()
                .UseSqlServer(config.GetConnectionString("Default")!, (opt) =>
                {
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    opt.MigrationsAssembly(typeof(TransactionDbContext).Assembly.FullName);
                });

            var services = new ServiceCollection();

            var servicesProvider = services.BuildServiceProvider();

            return new TransactionDbContext(builder.Options, new Mediator(servicesProvider));
        }

        private IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Nexa.Transactions.Application.Tests/"))
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}

