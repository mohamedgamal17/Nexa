using MediatR;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Domain;
namespace Vogel.BuildingBlocks.EntityFramework.Interceptors
{
    public class DispatchDomainEventInterceptor : SaveChangesInterceptor
    {
        private readonly IMediator _mediator;

        public DispatchDomainEventInterceptor(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            DispatchDomainEvents(eventData.Context).GetAwaiter().GetResult();

            return base.SavingChanges(eventData, result);
        }

        public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            await DispatchDomainEvents(eventData.Context);

            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }


        private async Task DispatchDomainEvents(DbContext? dbContext)
        {
            if (dbContext == null)
            {
                return;
            }

            var entries = dbContext.ChangeTracker.Entries();

            var aggregateRootEntries = dbContext.ChangeTracker.Entries<IAggregateRoot>();

            foreach (var entry in aggregateRootEntries)
            {
                if (entry.Entity.Events.Count > 0)
                {
                    foreach (var @event in entry.Entity.Events)
                    {
                        await _mediator.Publish(@event);

                        entry.Entity.ClearDomainEvents();

                    }
                }
            }
        }
    }
}
