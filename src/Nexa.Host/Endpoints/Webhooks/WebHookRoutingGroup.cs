using FastEndpoints;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
namespace Nexa.Host.Endpoints.Webhooks
{
    public class WebHookRoutingGroup : Group
    {

        public WebHookRoutingGroup()
        {
            Configure("webhooks", cfg =>
            {
                cfg.AllowAnonymous();
                cfg.Description(x =>
                    x.WithGroupName("WebHooks")
                    .Produces(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status401Unauthorized)
                    .Produces(StatusCodes.Status400BadRequest)
                    .Produces(StatusCodes.Status500InternalServerError));
            });
        }
    }


}
