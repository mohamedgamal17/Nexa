using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Application.Behaviours;
namespace Nexa.BuildingBlocks.Application.Extensions
{
    public static class MediatRServiceCollectionExtensions
    {
        public static IServiceCollection RegisterMediatRCommonPibelineBehaviors(this IServiceCollection services)
        {
            services.AddTransient(typeof(IRequestPostProcessor<,>), typeof(LoggingBehaviour<>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            return services;
        }
    }
}
