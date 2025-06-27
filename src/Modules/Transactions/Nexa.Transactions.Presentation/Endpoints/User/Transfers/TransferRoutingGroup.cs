using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Nexa.Transactions.Presentation.Endpoints.User.Transfers
{
    public class TransferRoutingGroup : Group
    {
        public TransferRoutingGroup()
        {
            Configure("user/transfers", ep =>
            {

                ep.Description(x => x.WithGroupName("Transfer")
                    .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status403Forbidden, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails))
                  );

            });
        }
    }
}
