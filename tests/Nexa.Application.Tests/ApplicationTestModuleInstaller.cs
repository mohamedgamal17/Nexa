using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nexa.Application.Tests.Services;
using Nexa.Application.Tests.Utilites;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Extensions;
using Nexa.BuildingBlocks.Infrastructure;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
namespace Nexa.Application.Tests
{
    public class ApplicationTestModuleInstaller : IModuleInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.
                InstallModule<InfrastructureModuleInstaller>(configuration, environment)
                .Replace<ISecurityContext, FakeSecurityContext>()
                .AddScoped<FakeAuthenticationService>();

            services.AddLogging()
                .AddTransient<ILogger, TestOutputLogger>()
                .AddSingleton<ILoggerFactory>(provider => new TestOutputLoggerFactory(true));

            services.RegisterMediatRCommonPibelineBehaviors()
                .RegisterEfCoreInterceptors();

            services.AddAuthorizationCore();
        }

    }
}
