using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Domain.NewFolder;
using Nexa.BuildingBlocks.Domain;
using System.Linq.Expressions;
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
