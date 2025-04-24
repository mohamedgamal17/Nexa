using MediatR;
using Nexa.BuildingBlocks.Domain.Results;

namespace Nexa.BuildingBlocks.Application.Requests
{
    public interface IApplicationReuest<T> :   IRequest<Result<T>>
    {
    }
}
