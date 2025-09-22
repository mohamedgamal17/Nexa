using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Domain.Dtos;
namespace Vogel.BuildingBlocks.EntityFramework.Extensions
{
    public static class IQueryableExtensions
    {
        public static async Task<Paging<T>> ToPaged<T>(this IQueryable<T> query, int skip,int length)
        {
            var result = await query.Skip(skip).Take(length).ToListAsync();

            var count = await query.CountAsync();

            var paging = new Paging<T>
            {
                Data= result,
                Info = new PagingInfo
                {
                    Skip = skip,
                    Length = length,
                    TotalCount = count
                }
            };
            return paging;
        }
    }
}
