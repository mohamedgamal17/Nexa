using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Nexa.BuildingBlocks.Infrastructure.Modularity
{
    public interface IServiceInstaller
    {
        void Install(IServiceCollection services, IConfiguration configuration, IHostEnvironment environment);
    }
}
