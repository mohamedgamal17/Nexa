using Microsoft.AspNetCore.Authorization;
using Nexa.Accounting.Domain.Wallets;
using Nexa.BuildingBlocks.Application.Security;

namespace Nexa.Accounting.Application.Wallets.Policies
{
    public class IsWalletOwnerAuthorizationRequirment : IAuthorizationRequirement
    {
    }
    public class IsWalletOwnerAuthorizationHandler : AuthorizationHandler<IsWalletOwnerAuthorizationRequirment, Wallet>
    {
        private readonly ISecurityContext _securityContext;

        public IsWalletOwnerAuthorizationHandler(ISecurityContext securityContext)
        {
            _securityContext = securityContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, IsWalletOwnerAuthorizationRequirment requirement, Wallet resource)
        {
            string userId = _securityContext.User!.Id;

            if(resource.UserId == userId)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
