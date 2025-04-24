using Nexa.BuildingBlocks.Domain.Results;
namespace Nexa.BuildingBlocks.Application.Requests
{
    public interface IApplicationRequestHandler<TRequest, TResult> : MediatR.IRequestHandler<TRequest, Result<TResult>>
       where TRequest : IApplicationReuest<TResult>
    {
    }
}
