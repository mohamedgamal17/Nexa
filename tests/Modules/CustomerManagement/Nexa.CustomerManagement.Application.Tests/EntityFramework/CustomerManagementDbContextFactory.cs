using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Infrastructure.EntityFramework;
namespace Nexa.CustomerManagement.Application.Tests.EntityFramework
{
    public class CustomerManagementDbContextFactory : IDesignTimeDbContextFactory<CustomerManagementDbContext>
    {
        public CustomerManagementDbContext CreateDbContext(string[] args)
        {
            var config = BuildConfiguration();

            var builder = new DbContextOptionsBuilder<CustomerManagementDbContext>()
                .UseSqlServer(config.GetConnectionString("Default")!, (opt) =>
                {
                    opt.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    opt.MigrationsAssembly(typeof(CustomerManagementDbContext).Assembly.FullName);
                });

            var services = new ServiceCollection();

            var servicesProvider = services.BuildServiceProvider();

            return new CustomerManagementDbContext(builder.Options, new Mediator(servicesProvider));
        }

        private IConfigurationRoot BuildConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Nexa.CustomerManagement.Application.Tests/"))
                .AddJsonFile("appsettings.json", false)
                .Build();
        }
    }
}

