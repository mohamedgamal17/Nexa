using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Infrastructure.Security;

namespace Nexa.BuildingBlocks.Infrastructure.Extensions
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection RegisterSecurityService(this IServiceCollection services)
        {
            return services.AddTransient<IApplicationAuthorizationService, ApplicationAuthorizationService>()
                .AddTransient<ISecurityContext, SecurityContext>();
        }
    }
}
