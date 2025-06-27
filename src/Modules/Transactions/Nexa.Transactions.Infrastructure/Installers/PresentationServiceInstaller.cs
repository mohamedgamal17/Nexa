using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nexa.BuildingBlocks.Infrastructure.Endpoints;
using Nexa.BuildingBlocks.Infrastructure.Modularity;

namespace Nexa.Transactions.Infrastructure.Installers
{
    public class PresentationServiceInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterEndpoints(Presentation.AssemblyReference.Assembly);
        }
    }
}
