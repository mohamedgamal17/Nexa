using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.BuildingBlocks.Application.Factories
{
    public interface IResponseFactory<TView, TDto>
      
    {
        Task<Paging<TDto>> PreparePagingDto(Paging<TView> paging);
        Task<List<TDto>> PrepareListDto(List<TView> views);
        Task<TDto> PrepareDto(TView view);
    }

    public abstract class ResponseFactory<TView, TDto> : IResponseFactory<TView, TDto>
        where TView : class
        where TDto : class
    {
        public virtual async Task<Paging<TDto>> PreparePagingDto(Paging<TView> paging)
        {
            var data = await PrepareListDto(paging.Data.ToList());

            var pagedDto = new Paging<TDto>
            {
                Data = data,
                Info = paging.Info
            };

            return pagedDto;
        }
        public virtual async Task<List<TDto>> PrepareListDto(List<TView> views)
        {
            var tasks = views.Select(PrepareDto);

            return (await Task.WhenAll(tasks)).ToList();
        }

        public abstract Task<TDto> PrepareDto(TView view); 
    }
}
