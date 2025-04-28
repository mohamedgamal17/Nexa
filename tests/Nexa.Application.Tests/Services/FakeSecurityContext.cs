using Nexa.BuildingBlocks.Application.Abstractions.Security;
using System.Security.Claims;

namespace Nexa.Application.Tests.Services
{
    internal class FakeSecurityContext : ISecurityContext
    {
        private readonly FakeAuthenticationService _userService;

        public FakeSecurityContext(FakeAuthenticationService userService)
        {
            _userService = userService;
        }

        public bool IsUserAuthenticated => _userService.GetCurrentUser() != null;

        public ApplicationUser? User => _userService.GetCurrentUser();

        public ClaimsPrincipal? ClaimsPrincipal => _userService.GetCurrentUserPrincibal();
    }
}
