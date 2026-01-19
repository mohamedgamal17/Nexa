using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.UploadDocumentAttachment;
using Nexa.CustomerManagement.Application.Tokens.Commands;
using Nexa.CustomerManagement.Application.Tokens.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Tokens
{
    public class KycSdkTokenEndpoint : Endpoint<CreateKycSdkTokenCommand, TokenDto>
    {
        private readonly IMediator _mediator;

        public KycSdkTokenEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("kyc/tokens");

            Description(x =>
                x.WithGroupName("Tokens")
                  .Produces(StatusCodes.Status200OK, typeof(TokenDto))
                  .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
                  .Produces(StatusCodes.Status403Forbidden, typeof(ProblemDetails))
                  .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
                  .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
            );

        }

        public override async Task HandleAsync(CreateKycSdkTokenCommand req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<CreateKycSdkTokenCommand>>();

            var validationResult = await validator.ValidateAsync(req);

            if (!validationResult.IsValid)
            {
                var errorResponse = validationResult.ValidationFailure();

                await SendResultAsync(errorResponse);

                return;
            }

            var result = await _mediator.Send(req);

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
