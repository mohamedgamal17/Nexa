using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Services;
using MediatR;
namespace Nexa.Application.Tests
{
    [SetUpFixture]
    public abstract class TestFixture
    {
        protected IServiceProvider ServiceProvider { get; private set; }
        protected IConfiguration Configuration { get; private set; }
        protected IMediator Mediator { get; private set; }
        protected FakeAuthenticationService AuthenticationService { get; private set; }
        protected abstract Task SetupAsync(IServiceCollection services, IConfiguration configuration);
        protected abstract Task InitializeAsync(IServiceProvider services);
        protected abstract Task ShutdownAsync(IServiceProvider services);

        protected TestFixture()
        {
            var services = new ServiceCollection();
            Configuration = BuildConfiguration();
            services.AddSingleton<IConfiguration>(Configuration);
            SetupAsync(services, Configuration).Wait();
            ServiceProvider = BuildServiceProvider(services);
        }


        [OneTimeSetUp]
        protected virtual async Task BeforeAnyTests()
        {
            Mediator = ServiceProvider.GetRequiredService<IMediator>();
            AuthenticationService = ServiceProvider.GetRequiredService<FakeAuthenticationService>();
            await InitializeAsync(ServiceProvider);
        }


        [OneTimeTearDown]
        protected virtual async Task TearDownAsync()
        {
            await ShutdownAsync(ServiceProvider);
        }

        private IConfiguration BuildConfiguration()
        {
            var builder = new ConfigurationManager()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", true, false)
                 .AddEnvironmentVariables();

            return builder.Build();
        }

        private IServiceProvider BuildServiceProvider(IServiceCollection services)
        {
            var containerBuilder = new ContainerBuilder();
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            var serviceProvider = new AutofacServiceProvider(container);
            return serviceProvider;
        }

        protected async Task<TResult> WithScopeAsync<TResult>(Func<IServiceProvider , Task<TResult>> func)
        {
            using var scope = ServiceProvider.CreateScope();

            return await func(scope.ServiceProvider);
        }
    }
}
