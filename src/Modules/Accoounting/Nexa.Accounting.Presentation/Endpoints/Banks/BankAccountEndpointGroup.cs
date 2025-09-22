using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Nexa.Accounting.Application.Tokens.Dtos;
namespace Nexa.Accounting.Presentation.Endpoints.Banks
{
    public class BankAccountEndpointGroup : Group
    {
        public BankAccountEndpointGroup()
        {
            Configure("user/banks", ep =>
            {

                ep.Description(x =>
                    x
                    .WithGroupName("Banks")
                    .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status403Forbidden, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails)));
            });
        }
    }
}
