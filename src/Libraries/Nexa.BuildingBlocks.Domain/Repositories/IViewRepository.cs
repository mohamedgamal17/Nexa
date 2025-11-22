using System.Linq.Expressions;

namespace Nexa.BuildingBlocks.Domain.Repositories
{
    public interface IViewRepository<TView> where TView : class
    {
        IQueryable<TView> QueryView();
        Task<TView?> SingleViewOrDefaultAsync(Expression<Func<TView, bool>> expression);
        Task<TView> SinglVieweAsync(Expression<Func<TView, bool>> expression);
    }
}
