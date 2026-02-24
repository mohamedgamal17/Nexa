using FastEndpoints;
using FluentValidation;
using MediatR;
using Nexa.Accounting.Application.Tokens.Commands.CreateLinkToken;
using Nexa.Accounting.Application.Tokens.Dtos;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
namespace Nexa.Accounting.Presentation.Endpoints.Banking
{
    public class CreateLinkTokenEndpoint : Endpoint<CreateLinkTokenCommand, EmptyResponse>
    {
        private readonly IMediator _mediator;
        public CreateLinkTokenEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("");

            Group<BankingEndpointGroup>();
        }
        public override async Task HandleAsync(CreateLinkTokenCommand req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<CreateLinkTokenCommand>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ToValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }

            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
