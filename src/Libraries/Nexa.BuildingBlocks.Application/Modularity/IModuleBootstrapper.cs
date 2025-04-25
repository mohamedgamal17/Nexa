namespace Nexa.BuildingBlocks.Application.Modularity
{
    public interface IModuleBootstrapper
    {
        Task Bootstrap(IServiceProvider serviceProvider);
    }
}
