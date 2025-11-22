using FastEndpoints;
using MediatR;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Reviews.Commands.CompleteKycReview;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Enums;
using System.Text.Json;
namespace Nexa.Host.Endpoints.Webhooks
{
    public class ComplyCubeWebHookRequest
    {
        public string Type { get; set; }
        public CheckPayload Payload { get; set; }


        public class CheckPayload
        {
            public string Id { get; set; }
            public string Status { get; set; }
            public string Outcome { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }

        }
    }
    public class ComplyCubeWebhookEndpoint : Endpoint<EmptyRequest>
    {

        private readonly IKYCProvider _kycProvider;

        private readonly IMediator _mediator;

        private readonly ILogger<ComplyCubeWebhookEndpoint> _logger;

        private Dictionary<string, Func<ComplyCubeWebHookRequest, Task>> _eventHandlers;

        public ComplyCubeWebhookEndpoint(IKYCProvider kycProvider, IMediator mediator, ILogger<ComplyCubeWebhookEndpoint> logger)
        {
            _kycProvider = kycProvider;
            _mediator = mediator;
            _eventHandlers = new Dictionary<string, Func<ComplyCubeWebHookRequest, Task>>
            {
                {"check.completed", HandleCheckCompletedEvent }
            };
            _logger = logger;
        }

        public override void Configure()
        {
            Post("complycube");
            Group<WebhookRoutingGroup>();
            DontAutoTag();
        }


        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var isSignatureExist =
                HttpContext.Request.Headers.TryGetValue("complycube-signature", out var signature);

            var body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            if (!isSignatureExist)
            {
                await SendUnauthorizedAsync(ct);

                return;
            }

            var isValidSignature = await _kycProvider.VerifiyWebHookSignature(signature!, body);

            if (!isValidSignature)
            {
                await SendUnauthorizedAsync(ct);

                return;
            }

            var request = JsonSerializer.Deserialize<ComplyCubeWebHookRequest>(body, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase })!;


            _logger.LogDebug("Recived complycube webhook event ({eventType}).\n body : {@event}", request.Type, request);


            if (_eventHandlers.ContainsKey(request.Type))
            {
                await _eventHandlers[request.Type].Invoke(request);
            }

            await SendOkAsync();
        }

        private async Task<Result<Unit>> HandleCheckCompletedEvent(ComplyCubeWebHookRequest request)
        {
            var command = new CompleteKycReviewCommand
            {
                KycCheckId = request.Payload.Id,
                Outcome = MapKycReviewOutcome(request.Payload.Outcome)
            };

            return await _mediator.Send(command);
        }

        private KycReviewOutcome MapKycReviewOutcome(string outcome)
            => outcome switch
            {
                "clear" => KycReviewOutcome.Clear,
                _ => KycReviewOutcome.Rejected
            };
    }
}
