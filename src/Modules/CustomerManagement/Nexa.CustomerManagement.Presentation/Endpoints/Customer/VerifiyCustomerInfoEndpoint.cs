using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using Nexa.BuildingBlocks.Infrastructure.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.VerifyCustomerInfo;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class VerifiyCustomerInfoEndpoint : Endpoint<EmptyRequest, CustomerDto>
    {
        private readonly IMediator _mediator;

        public VerifiyCustomerInfoEndpoint(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override void Configure()
        {
            Post("info/verifiy");

            Description(x => x.Produces(StatusCodes.Status200OK, typeof(CustomerDto)));

            Group<CustomerRoutingGroup>();
        }

        public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
        {
            var result = await _mediator.Send(new VerifyCustomerInfoCommand() );

            var response = result.ToOk();

            await SendResultAsync(response);
        }
    }
}
