using FastEndpoints;
using MediatR;
using Nexa.CustomerManagement.Application.Customers.Commands.AcceptCustomer;
using Nexa.CustomerManagement.Application.Customers.Commands.RejectCustomer;
using Nexa.Integrations.Baas.Abstractions.Contracts.Events;
using Nexa.Integrations.Baas.Abstractions.Services;

namespace Nexa.Host.Endpoints.Webhooks
{
    public class StripeWebHookEndpoint : Endpoint<EmptyRequest>
    {
        private readonly IBaasWebHookService _baasWebhookService;
        private readonly IMediator _mediator;
        public StripeWebHookEndpoint(IBaasWebHookService baasWebhookService, IMediator mediator)
        {
            _baasWebhookService = baasWebhookService;
            _mediator = mediator;
        }


        public override void Configure()
        {
            Post("webhooks/stripe");
            Group<WebHookRoutingGroup>();
        }
        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            string signature = HttpContext.Request!.Headers["Stripe-Signature"]!;

            var isValid = await _baasWebhookService.VerifiySignature(signature, json);

            if (isValid)
            {             
                var stripeEvent = await _baasWebhookService.ConstructEvent(json);

                if (stripeEvent.Type == Stripe.EventTypes.AccountUpdated)
                {
                    await HandleAccountUpdateEvent(stripeEvent);
                }

                await SendOkAsync();
            }
            else
            {
                await SendUnauthorizedAsync();

            }        
        }

        private async Task HandleAccountUpdateEvent(Event stripeEvent)
        {
            var stripeEntity = (Stripe.Account)stripeEvent.Data;

            if (stripeEntity.Individual.Verification.Status == "verified")
            {
                var command = new AcceptCustomerCommand { FintechCustomerId = stripeEntity.Id };

                await _mediator.Send(command);

            }
            else if (stripeEntity.Individual.Verification.Status == "unverified")
            {
                if (stripeEntity.Individual.Verification.DetailsCode != null)
                {
                    var command = new RejectCustomerCommand { FintechCustomerId = stripeEntity.Id };

                    await _mediator.Send(command);
                }
            }
        }
    }
}
