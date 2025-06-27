using FastEndpoints;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Nexa.BuildingBlocks.Application.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Endpoints;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.BuildingBlocks.Infrastructure.Modularity;
namespace Nexa.Host
{
    public class HostModuleInstaller : IModuleInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            ConfigureAuthentication(services, configuration);

            ConfigureAuthorization(services);


            ConfigureMassTransit(services, configuration);

            ConfigureSwagger(services);

            ConfigureSignalRHubs(services);

            RegisterControllers(services);

            services
                .RegisterMediatRCommonPibelineBehaviors()
                .RegisterSecurityService()
                .AddHttpContextAccessor()
                .InstallModulesFromAssembly(
                    configuration,
                     Accounting.Infrastructure.AssemblyReference.Assembly,
                     CustomerManagement.Infrastructure.AssemblyReference.Assembly,
                     Transactions.Infrastructure.AssemblyReference.Assembly
                );

            ConfigureFastEndpoint(services);

        }

        private void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.Authority = configuration.GetValue<string>("IdentityProvider:Authority");
                options.Audience = configuration.GetValue<string>("IdentityProvider:Audience");
            });
        }

        private void ConfigureAuthorization(IServiceCollection services)
        {
            services.AddAuthorization();
        }


        private void ConfigureMassTransit(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(busRegisterConfig =>
            {

                busRegisterConfig.AddConsumers(
                    Transactions.Application.AssemblyReference.Assembly,
                    Accounting.Application.AssemblyReference.Assembly
                    );

                busRegisterConfig.UsingRabbitMq((ctx, rabbitMqConfig) =>
                {
                    string rabbitMqHost = configuration.GetValue<string>("RabbitMq:Host")!;
                    string userName = configuration.GetValue<string>("RabbitMq:UserName")!;
                    string password = configuration.GetValue<string>("RabbitMq:Password")!;

                    
                    rabbitMqConfig.Host(rabbitMqHost, hostConfig =>
                    {
                        hostConfig.Username(userName);
                        hostConfig.Password(password);
                    });

                    rabbitMqConfig.ConfigureEndpoints(ctx);
                });

            });
        }

        private void RegisterControllers(IServiceCollection services)
        {
            services.AddControllers();
        }
        private void ConfigureSwagger(IServiceCollection services)
        {
            services.SwaggerDocument(opt =>
            {
                opt.AutoTagPathSegmentIndex = 2;
            });
        }

        private void ConfigureSignalRHubs(IServiceCollection services)
        {
            services.AddSignalR();
        }

        private void ConfigureFastEndpoint(IServiceCollection services)
        {
            var registery = services.GetSinglatonOrNull<EndpointAssemblyRegistery>()
                ?? new EndpointAssemblyRegistery();

            var assemblies = registery.Assemblies;

            services.AddFastEndpoints(opt =>
            {
                opt.Assemblies = assemblies;
                opt.IncludeAbstractValidators = true;
            });
        }
    }
}

