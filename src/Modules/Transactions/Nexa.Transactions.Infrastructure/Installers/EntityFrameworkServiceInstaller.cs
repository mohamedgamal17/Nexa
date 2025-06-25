using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.Transactions.Domain;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Nexa.Transactions.Infrastructure.EntityFramework.Repositories;
namespace Nexa.Transactions.Infrastructure.Installers
{
    public class EntityFrameworkServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<TransactionDbContext>((sp, opt) =>
            {
                opt.UseSqlServer(configuration.GetConnectionString("Default"), cfg => cfg
                    .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)
                   );
            });

            services.AddTransient(typeof(ITransactionRepository<>), typeof(TransactionRepository<>))
                .AddTransient<ITransferRepository, TransferRepository>();
        }
    }
}
