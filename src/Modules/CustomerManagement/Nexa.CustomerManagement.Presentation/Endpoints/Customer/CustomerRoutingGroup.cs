using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Customer
{
    public class CustomerRoutingGroup : Group
    {
        public CustomerRoutingGroup()
        {
            Configure("user/customer", ep =>
            {
      
                ep.Description(x =>
                    x
                    .WithGroupName("Customers")
                    .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status403Forbidden, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails)));
            });
        }
    }
}
