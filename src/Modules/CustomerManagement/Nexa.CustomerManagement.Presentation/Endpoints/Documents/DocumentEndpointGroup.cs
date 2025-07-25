using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Nexa.CustomerManagement.Presentation.Endpoints.Documents
{
    public class DocumentEndpointGroup : Group
    {
        public DocumentEndpointGroup()
        {
            Configure("user/customers/documents", ep =>
            {
                ep.Description(x =>
                  x
                  .WithGroupName("Documents")
                  .Produces(StatusCodes.Status401Unauthorized, typeof(ProblemDetails))
                  .Produces(StatusCodes.Status403Forbidden, typeof(ProblemDetails))
                  .Produces(StatusCodes.Status404NotFound, typeof(ProblemDetails))
                  .Produces(StatusCodes.Status500InternalServerError, typeof(ProblemDetails)));
            });
        }
    }
}
