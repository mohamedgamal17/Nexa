using MediatR;
using Nexa.BuildingBlocks.Domain.Results;
namespace Nexa.BuildingBlocks.Application.Requests
{
    public interface IApplicationPiplineBehaviour<TRequest, TResponse> 
        : IPipelineBehavior<TRequest, Result<TResponse>>
        where TRequest : notnull
    {

    }
}
