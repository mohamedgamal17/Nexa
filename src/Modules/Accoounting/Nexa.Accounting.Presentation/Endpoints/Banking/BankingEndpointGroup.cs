using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Nexa.Accounting.Application.Tokens.Dtos;
namespace Nexa.Accounting.Presentation.Endpoints.Banking
{
    public class BankingEndpointGroup : Group
    {
        public BankingEndpointGroup()
        {
            Configure("banking/tokens", ep =>
            {

                ep.Description(x =>
                    x
                    .WithGroupName("Banking Tokens")
                    .Produces(StatusCodes.Status200OK, typeof(BankingTokenDto))
                    .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status403Forbidden, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails)));
            });
        }
    }
}
