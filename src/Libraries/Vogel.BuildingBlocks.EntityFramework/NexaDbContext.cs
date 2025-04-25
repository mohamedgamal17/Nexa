using Microsoft.EntityFrameworkCore;
namespace Vogel.BuildingBlocks.EntityFramework
{
    public abstract class NexaDbContext<TContext> : DbContext where TContext : DbContext
    {
        public NexaDbContext(DbContextOptions<TContext> options) : base(options)
        {

        }
    }
}
