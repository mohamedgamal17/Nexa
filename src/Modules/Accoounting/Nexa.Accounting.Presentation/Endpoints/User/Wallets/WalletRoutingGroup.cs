using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Nexa.Accounting.Presentation.Endpoints.User.Wallets
{
    public class WalletRoutingGroup : Group
    {
        public WalletRoutingGroup()
        {
            Configure("user/wallets", ep =>
            {

                ep.Description(x =>
                    x
                    .WithGroupName("Wallets")
                    .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status403Forbidden, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails)));
            });
        }
    }
}
