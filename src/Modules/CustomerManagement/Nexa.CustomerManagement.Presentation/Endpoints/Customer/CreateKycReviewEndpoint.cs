using FastEndpoints;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer;
using Nexa.CustomerManagement.Application.Reviews.Commands.CreateKycReview;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class CreateKycReviewEndpoint : Endpoint<CreateKycReviewCommand, KycReviewDto>
    {
        private readonly IMediator _mediator;

        public CreateKycReviewEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("reviews");

            Description(x =>
                x.Produces(StatusCodes.Status200OK, typeof(KycReviewDto))
            );

            Group<CustomerRoutingGroup>();

        }

        public override async Task HandleAsync(CreateKycReviewCommand req, CancellationToken ct)
        {
            var validator = Resolve<IValidator<CreateKycReviewCommand>>();

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
