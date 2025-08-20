using Nexa.Integrations.Baas.Abstractions.Contracts.Events;

namespace Nexa.Integrations.Baas.Abstractions.Services
{
    public interface IBaasWebHookService
    {
        Task<bool> VerifiySignature(string signature, string body);

        Task<Event> ConstructEvent(string body);
    }
}
