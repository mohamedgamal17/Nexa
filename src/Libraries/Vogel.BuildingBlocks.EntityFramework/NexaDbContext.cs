using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Domain;
namespace Vogel.BuildingBlocks.EntityFramework
{
    public abstract class NexaDbContext<TContext> : DbContext where TContext : DbContext
    {
        protected  IMediator Mediator { get; set; }

        public NexaDbContext(DbContextOptions<TContext> options , IMediator mediator)
            : base(options)
        {
            Mediator = mediator;
        }

        public NexaDbContext(DbContextOptions options) : base(options)
        {
        }

        public override  int SaveChanges()
        {
            var result = base.SaveChanges();

            DispatchDomainEvents().Wait();

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            await DispatchDomainEvents();

            return result;
        }


        private async Task DispatchDomainEvents()
        {
            var entities = ChangeTracker
            .Entries<IAggregateRoot>()
            .Where(e => e.Entity.Events.Any())
            .Select(e => e.Entity)
            .ToList();
            
            var events = entities.SelectMany(x => x.Events).ToList();

            foreach (var entity in entities)
            {
                entity.ClearDomainEvents();
            }

            foreach (var domainEvent in events)
            {
                await Mediator.Publish(domainEvent);
            }
        }
    }
}
