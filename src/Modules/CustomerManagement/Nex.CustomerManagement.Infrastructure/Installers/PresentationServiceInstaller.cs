using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Endpoints;
using Nexa.BuildingBlocks.Infrastructure.Modularity;

namespace Nexa.CustomerManagement.Infrastructure.Installers
{
    public class PresentationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddValidatorsFromAssembly(Application.AssemblyReference.Assembly)
                .RegisterEndpoints(Presentation.AssemblyReference.Assembly); 
        }
    }
}
