using FastEndpoints;
using MediatR;
using Nexa.Integrations.Baas.Abstractions.Contracts.Events;
using Nexa.Integrations.Baas.Abstractions.Services;
using Nexa.Integrations.Baas.Stripe;
using Nexa.Transactions.Shared.Events;
using Stripe.Treasury;

namespace Nexa.Host.Endpoints.Webhooks
{
    public class StripeWebhookEndpoint : Endpoint<EmptyRequest>
    {
        private readonly IBaasWebHookService _baasWebhookService;
        private readonly IMediator _mediator;
        private readonly MassTransit.IPublishEndpoint _publishEndpoint;
        private readonly ILogger<StripeWebhookEndpoint> _logger;
        public StripeWebhookEndpoint(IBaasWebHookService baasWebhookService, IMediator mediator, ILogger<StripeWebhookEndpoint> logger, MassTransit.IPublishEndpoint publishEndpoint)
        {
            _baasWebhookService = baasWebhookService;
            _mediator = mediator;
            _logger = logger;
            _publishEndpoint = publishEndpoint;  
        }


        public override void Configure()
        {
            Post("stripe");
            Group<WebhookRoutingGroup>();
            DontAutoTag();
        }
        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            string signature = HttpContext.Request!.Headers["Stripe-Signature"]!;

            var isValid = await _baasWebhookService.VerifiySignature(signature, json);

            if (isValid)
            {             
                var stripeEvent = await _baasWebhookService.ConstructEvent(json);

                _logger.LogDebug("Recived stripe webhook event ({eventType}).\n body : {@event}", stripeEvent.Type, stripeEvent);


                if (stripeEvent.Type.Contains("inbound_transfer"))
                {
                    _logger.LogDebug("Handling stripe inbound transfer");

                    await HandleInboundTransfer(stripeEvent);
                }

                if (stripeEvent.Type.Contains("outbound_transfer"))
                {
                    _logger.LogDebug("Handling stripe outbound transfer");

                    await HandleOutboundTransfer(stripeEvent);
                }

                if (stripeEvent.Type.Contains("outbound_payment"))
                {
                    _logger.LogDebug("Handling stripe outbound payment");

                    await HandleOutboundPayment(stripeEvent);
                }

                await SendOkAsync();
            }
            else
            {
                await SendUnauthorizedAsync();

            }        
        }


        private async Task HandleInboundTransfer(Event stripeEvent)
        {
            var stripeEntity = (InboundTransfer)stripeEvent.Data;

            if(stripeEvent.Type == Stripe.EventTypes.TreasuryInboundTransferSucceeded)
            {
                var @event = new ExternalTransferCompletedIntegrationEvent
                {
                    TransferId = stripeEntity.Metadata[StripeMetaDataConsts.ClientTransferId],
                    ExternalTransferId = stripeEntity.Id

                };

                await _publishEndpoint.Publish(@event);
            }         
        }

        private async Task HandleOutboundTransfer(Event stripeEvent)
        {
            var stripeEntity = (OutboundTransfer)stripeEvent.Data;

            if (stripeEvent.Type == Stripe.EventTypes.TreasuryOutboundTransferPosted)
            {
                var @event = new ExternalTransferCompletedIntegrationEvent
                {
                    TransferId = stripeEntity.Metadata[StripeMetaDataConsts.ClientTransferId],
                    ExternalTransferId = stripeEntity.Id

                };

                await _publishEndpoint.Publish(@event);
            }

        }

        private async  Task HandleOutboundPayment(Event stripeEvent)
        {
            var stripeEntity = (OutboundPayment)stripeEvent.Data;

            if(stripeEvent.Type == Stripe.EventTypes.TreasuryOutboundPaymentPosted)
            {
                var @event = new ExternalTransferCompletedIntegrationEvent
                {
                    TransferId = stripeEntity.Metadata[StripeMetaDataConsts.ClientTransferId],
                    ExternalTransferId = stripeEntity.Id
                };

                await _publishEndpoint.Publish(@event);
            }
        }
    }
}
