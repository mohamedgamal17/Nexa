using Nexa.Integrations.Baas.Abstractions.Configuration;
using Nexa.Integrations.Baas.Abstractions.Services;
using Stripe;
namespace Nexa.Integrations.Baas.Stripe
{
    public class StripeWebhookService : IBaasWebHookService
    {
        private readonly BaasConfiguration _configuration;

        public StripeWebhookService(BaasConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<Abstractions.Contracts.Events.Event> ConstructEvent(string body)
        {
            var stripeEvent = EventUtility.ParseEvent(body, false);

            var @event = new Abstractions.Contracts.Events.Event
            {
                Id = stripeEvent.Id,
                Data = stripeEvent.Data.Object,
                Type = stripeEvent.Type
            };

            return Task.FromResult(@event);
        }

        public Task<bool> VerifiySignature(string signature, string body)
        {
           
            try
            {
                EventUtility.ValidateSignature(body, signature, _configuration.WebhookSecret);

                return Task.FromResult(true);

            }catch(Exception ex)
            {
                return Task.FromResult(false);
            }

        }
    }
}
