using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nexa.BuildingBlocks.Domain;
using Nexa.BuildingBlocks.Domain.Events;
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

            var entries = ChangeTracker
               .Entries<IAggregateRoot>()
               .Where(e => e.Entity.Events.Any())
               .ToList();

            DispatchDomainEvents(entries).Wait();

            PublishBasicCrudEvents(entries).Wait();

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var result = await base.SaveChangesAsync(cancellationToken);

            var entries = ChangeTracker
               .Entries()
               .ToList();

            await DispatchDomainEvents(entries);

            await PublishBasicCrudEvents(entries);

            return result;
        }


        private async Task DispatchDomainEvents(IEnumerable<EntityEntry> entries)
        {
            var entities = entries
                .Select(x => x.Entity)
                .OfType<IAggregateRoot>()
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

        private async Task PublishBasicCrudEvents(IEnumerable<EntityEntry> entries)
        {
            foreach (var entry in entries)
            {
                var entityType = entry.Entity.GetType();

                if (entry.State == EntityState.Added)
                {
                    var eventType = typeof(EntityCreatedEvent<>).MakeGenericType(entityType);

                    var @event = Activator.CreateInstance(eventType, new object[] { entry.Entity })!;

                    await Mediator.Publish(@event);

                }
                else if (entry.State == EntityState.Modified)
                {
                    var eventType = typeof(EntityUpdatedEvent<>).MakeGenericType(entityType);

                    var @event = Activator.CreateInstance(eventType, new object[] { entry.Entity })!;

                    await Mediator.Publish(@event);
                }
                else if (entry.State == EntityState.Deleted)
                {
                    var eventType = typeof(EntityDeletedEvent<>).MakeGenericType(entityType);

                    var @event = Activator.CreateInstance(eventType, new object[] { entry.Entity })!;

                    await Mediator.Publish(@event);
                }

            }
        }
    }
}
