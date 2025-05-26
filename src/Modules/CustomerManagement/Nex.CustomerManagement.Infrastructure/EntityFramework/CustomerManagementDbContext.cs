using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexa.CustomerManagement.Infrastructure;
using System.Reflection;
using Vogel.BuildingBlocks.EntityFramework;

namespace Nexa.CustomerManagement.Infrastructure.EntityFramework
{
    public class CustomerManagementDbContext : NexaDbContext<CustomerManagementDbContext>
    {
        public CustomerManagementDbContext(DbContextOptions<CustomerManagementDbContext> options,
            IMediator mediator) : base(options, mediator)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(CustomerManagementDbConstants.Schema);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }

    }
}
