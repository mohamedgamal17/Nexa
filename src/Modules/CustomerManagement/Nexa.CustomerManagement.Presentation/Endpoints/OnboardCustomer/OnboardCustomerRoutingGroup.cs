using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Nexa.CustomerManagement.Presentation.Endpoints.OnboardCustomer
{
    public class OnboardCustomerRoutingGroup : Group
    {
        public OnboardCustomerRoutingGroup()
        {
            Configure("user/onboardcustomer", ep =>
            {
      
                ep.Description(x =>
                    x
                    .WithTags("OnboardCustomers")
                    .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status403Forbidden, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
                    .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails)));
            });
        }
    }
}
