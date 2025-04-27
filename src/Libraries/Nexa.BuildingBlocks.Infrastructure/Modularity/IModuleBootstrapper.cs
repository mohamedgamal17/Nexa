namespace Nexa.BuildingBlocks.Infrastructure.Modularity
{
    public interface IModuleBootstrapper
    {
        Task Bootstrap(IServiceProvider serviceProvider);
    }
}
