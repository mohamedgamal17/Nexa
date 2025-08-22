using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Nexa.BuildingBlocks.Domain;
using Nexa.BuildingBlocks.Domain.Events;
using System.Threading;
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
            var entries = ChangeTracker
                      .Entries()
                     .ToList();
            var crudEvents = ExtractCrudEvent(entries);

            var domainEvents = ExtractDomainEvents(entries);

            var allEvents = Enumerable.Concat(crudEvents, domainEvents);

            var result = base.SaveChanges();

            PublishDomainEvents(allEvents).Wait();

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .ToList();
            
            var crudEvents = ExtractCrudEvent(entries);
 
            var domainEvents = ExtractDomainEvents(entries);

            var allEvents = Enumerable.Concat(crudEvents, domainEvents);

            var result = await base.SaveChangesAsync(cancellationToken);

            await PublishDomainEvents(allEvents);

            return result;
        }


        private List<IEvent> ExtractDomainEvents(IEnumerable<EntityEntry> entries)
        {
            var entities = entries
                .Select(x => x.Entity)
                .OfType<IAggregateRoot>()
                .Where(x=> x.Events.Any())
                .ToList();
            
            var events = entities.SelectMany(x => x.Events).ToList();

            foreach (var entity in entities)
            {
                entity.ClearDomainEvents();
            }

            return events;
        }


        public List<IEvent> ExtractCrudEvent(IEnumerable<EntityEntry> entries)
        {
            return entries
                  .Select(CreateCrudEvent)
                  .Where(x => x != null)
                  .ToList()!;
        }

        private static IEvent? CreateCrudEvent(EntityEntry entry)
        {
            var entityType = entry.Entity.GetType();

            return entry.State switch
            {
                EntityState.Added => 
                 (IEvent?)Activator.CreateInstance(typeof(EntityCreatedEvent<>).MakeGenericType(entityType), entry.Entity),
                EntityState.Modified =>
                 (IEvent?) Activator.CreateInstance(typeof(EntityUpdatedEvent<>).MakeGenericType(entityType), entry.Entity),
                EntityState.Deleted =>
                (IEvent?) Activator.CreateInstance(typeof(EntityDeletedEvent<>).MakeGenericType(entityType), entry.Entity),
                _ => null
            };
        }

        private async Task PublishDomainEvents(IEnumerable<IEvent> events)
        {
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
