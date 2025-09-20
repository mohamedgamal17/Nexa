using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nexa.Application.Tests.Providers.Baas;
using Nexa.Application.Tests.Providers.OpenBanking;
using Nexa.Application.Tests.Services;
using Nexa.Application.Tests.Utilites;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Extensions;
using Nexa.BuildingBlocks.Infrastructure;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
using Nexa.Integrations.Baas.Abstractions.Services;
using Nexa.Integrations.OpenBanking.Abstractions;
namespace Nexa.Application.Tests
{
    public class ApplicationTestModuleInstaller : IModuleInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.
                InstallModule<InfrastructureModuleInstaller>(configuration)
                .Replace<ISecurityContext, FakeSecurityContext>()
                .AddScoped<FakeAuthenticationService>();

            services.AddLogging()
                .AddTransient<ILogger, TestOutputLogger>()
                .AddSingleton<ILoggerFactory>(provider => new TestOutputLoggerFactory(true));

            services.RegisterMediatRCommonPibelineBehaviors()
                .RegisterEfCoreInterceptors();

            services.AddAuthorizationCore();

            RegisterFakeBaasProvider(services);

            RegisterFakeOpenBanking(services);

        }

        private void RegisterFakeBaasProvider(IServiceCollection services)
        {
            services.AddTransient<IBaasClientService, FakeBaasClientService>()
                .AddTransient<IBaasWalletService, FakeBaasWalletProvider>()
                .AddTransient<IBaasFundingResourceService, FakeBaasFundingResurceService>()
                .AddTransient<IBaasTransferService, FakeBaasTransferService>();
        }

        private void RegisterFakeOpenBanking(IServiceCollection services)
        {
            services.AddTransient<IBankingTokenService, FakeBankingTokenService>();
        }

    }
}
