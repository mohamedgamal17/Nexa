using System.Security.Claims;
namespace Nexa.BuildingBlocks.Application.Abstractions.Security
{
    public interface ISecurityContext
    {
        bool IsUserAuthenticated { get; }
        ApplicationUser? User { get; }
        ClaimsPrincipal? ClaimsPrincipal { get; }
    }
}
