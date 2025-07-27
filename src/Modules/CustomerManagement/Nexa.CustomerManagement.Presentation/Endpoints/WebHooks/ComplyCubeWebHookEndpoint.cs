using FastEndpoints;
using MediatR;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Reviews.Commands.UpdateKycReview;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Enums;
using System.Text.Json;
namespace Nexa.CustomerManagement.Presentation.Endpoints.WebHooks
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
    public class ComplyCubeWebHookEndpoint : Endpoint<EmptyRequest>
    {

        private readonly IKYCProvider _kycProvider;

        private readonly IMediator _mediator;

        private Dictionary<string, Func<ComplyCubeWebHookRequest, Task>> _eventHandlers;

        public ComplyCubeWebHookEndpoint(IKYCProvider kycProvider, IMediator mediator)
        {
            _kycProvider = kycProvider;
            _mediator = mediator;
            _eventHandlers = new Dictionary<string, Func<ComplyCubeWebHookRequest, Task>>
            {
                {"check.completed", HandleCheckCompletedEvent }
            };
        }

        public override void Configure()
        {
            Post("complycube");

            Group<WebHookRoutingGroup>();
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
