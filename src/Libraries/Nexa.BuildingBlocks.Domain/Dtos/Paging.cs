namespace Nexa.BuildingBlocks.Domain.Dtos
{
    public class Paging<T>
    {
        public IEnumerable<T> Data { get; set; }
        public PagingInfo Info { get; }      
    }
}
