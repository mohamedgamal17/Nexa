using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Domain;
using System.Linq.Expressions;
using Nexa.BuildingBlocks.Domain.Repositories;
namespace Vogel.BuildingBlocks.EntityFramework.Repositories
{
    public abstract class EFCoreViewRepository<TEntity, TView, TContext> : EFCoreRepository<TEntity, TContext>, IViewRepository<TView>
         where TEntity : class, IEntity
         where TView : class
         where TContext : DbContext
    {
        public EFCoreViewRepository(TContext dbContext) : base(dbContext)
        {
        }

        public abstract IQueryable<TView> QueryView();
     

        public Task<TView?> SingleViewOrDefaultAsync(Expression<Func<TView, bool>> expression)
        {
            return QueryView().SingleOrDefaultAsync(expression);
        }

        public Task<TView> SinglVieweAsync(Expression<Func<TView, bool>> expression)
        {
            return QueryView().SingleAsync(expression);
        }
    }
}
