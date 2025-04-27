using MediatR;
using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;

namespace Nexa.BuildingBlocks.Infrastructure.Security
{
    internal class ApplicationAuthorizationService
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ISecurityContext _securityContext;
        public ApplicationAuthorizationService(IAuthorizationService authorizationService, ISecurityContext securityContext)
        {
            _authorizationService = authorizationService;
            _securityContext = securityContext;
        }

        public async Task<Result<Unit>> AuthorizeAsync(IAuthorizationRequirement requirement)
        {
            if (!_securityContext.IsUserAuthenticated)
            {
                return new Result<Unit>(new UnauthorizedAccessException());
            }

            var currentUser = _securityContext.ClaimsPrincipal!;

            var authorizationResult = await _authorizationService.AuthorizeAsync(currentUser, null, requirement);

            if (!authorizationResult.Succeeded)
            {
                return PrepareFailureAuthroizationResult(authorizationResult.Failure!);
            }

            return Unit.Value;
        }

        public async Task<Result<Unit>> AuthorizeAsync(object resource, IAuthorizationRequirement requirements)
        {
            if (!_securityContext.IsUserAuthenticated)
            {
                return new Result<Unit>(new UnauthorizedAccessException());
            }

            var currentUser = _securityContext.ClaimsPrincipal!;

            var authorizationResult = await _authorizationService.AuthorizeAsync(currentUser, resource, requirements);

            if (!authorizationResult.Succeeded)
            {
                return PrepareFailureAuthroizationResult(authorizationResult.Failure!);
            }

            return Unit.Value;
        }

        public async Task<Result<Unit>> AuthorizeAsync(object resource, string policyName)
        {

            if (!_securityContext.IsUserAuthenticated)
            {
                return new Result<Unit>(new UnauthorizedAccessException());
            }

            var currentUser = _securityContext.ClaimsPrincipal!;

            var authorizationResult = await _authorizationService.AuthorizeAsync(currentUser, resource, policyName);

            if (!authorizationResult.Succeeded)
            {
                return PrepareFailureAuthroizationResult(authorizationResult.Failure!);
            }

            return Unit.Value;
        }

        public async Task<Result<Unit>> AuthorizeAsync(string policyName)
        {
            if (!_securityContext.IsUserAuthenticated)
            {
                return new Result<Unit>(new UnauthorizedAccessException());
            }

            var currentUser = _securityContext.ClaimsPrincipal!;

            var authorizationResult = await _authorizationService.AuthorizeAsync(currentUser, policyName);


            if (!authorizationResult.Succeeded)
            {
                return PrepareFailureAuthroizationResult(authorizationResult.Failure!);
            }

            return Unit.Value;
        }


        private Result<Unit> PrepareFailureAuthroizationResult(AuthorizationFailure authorizationFailure)
        {
            var failureMessages = authorizationFailure
                .FailureReasons
                .Select(x => x.Message)
                .ToList();

            string message = string.Join(" , ", failureMessages);

            return new Result<Unit>(new ForbiddenAccessException(message));
        }
    }
}
