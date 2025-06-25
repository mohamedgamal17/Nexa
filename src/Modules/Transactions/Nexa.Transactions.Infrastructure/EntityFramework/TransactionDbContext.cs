using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Vogel.BuildingBlocks.EntityFramework;

namespace Nexa.Transactions.Infrastructure.EntityFramework
{
    public class TransactionDbContext : NexaDbContext<TransactionDbContext>
    {
        public TransactionDbContext(DbContextOptions<TransactionDbContext> options, IMediator mediator) : base(options, mediator)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
