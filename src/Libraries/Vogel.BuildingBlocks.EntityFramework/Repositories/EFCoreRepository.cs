using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Domain.NewFolder;
using Nexa.BuildingBlocks.Domain;
using System.Linq.Expressions;
namespace Vogel.BuildingBlocks.EntityFramework.Repositories
{
    public abstract class EFCoreRepository<TEntity, TContext> : IRepository<TEntity>
         where TEntity : class, IEntity
         where TContext : DbContext
    {
        protected  TContext DbContext { get; }

        public EFCoreRepository(TContext dbContext)
        {
            DbContext = dbContext;
        }

        public IQueryable<TEntity> AsQuerable()
        {
            return DbContext.Set<TEntity>().AsQueryable();
        }

        public async Task DeleteAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Remove(entity);
            await DbContext.SaveChangesAsync();
        }

        public async Task<TEntity?> FindByIdAsync(object id)
        {
            return await DbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            await DbContext.Set<TEntity>().AddAsync(entity);

            await DbContext.SaveChangesAsync();

            return entity;
        }

        public async Task<List<TEntity>> InsertManyAsync(List<TEntity> entities)
        {
            await DbContext.Set<TEntity>().AddRangeAsync(entities);

            await DbContext.SaveChangesAsync();

            return entities;
        }

        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbContext.Set<TEntity>().SingleAsync(expression);
        }

        public async Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbContext.Set<TEntity>().SingleOrDefaultAsync(expression);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            DbContext.Set<TEntity>().Update(entity);

            await DbContext.SaveChangesAsync();

            return entity;
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> expression)
        {
            return await DbContext.Set<TEntity>().AnyAsync(expression);
        }

    }
}
