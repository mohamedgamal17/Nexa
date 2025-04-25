using MediatR;
using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Domain.Results;
namespace Nexa.BuildingBlocks.Application.Security
{
    public interface IApplicationAuthorizationService
    {
        Task<Result<Unit>> AuthorizeAsync(IAuthorizationRequirement requirement);
        Task<Result<Unit>> AuthorizeAsync(object resource, IAuthorizationRequirement requirements);
        Task<Result<Unit>> AuthorizeAsync(object resource, string policyName);
        Task<Result<Unit>> AuthorizeAsync(string policyName);
    }
}
