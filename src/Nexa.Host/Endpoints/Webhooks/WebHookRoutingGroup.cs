using FastEndpoints;
namespace Nexa.Host.Endpoints.Webhooks
{
    public class WebhookRoutingGroup : Group
    {

        public WebhookRoutingGroup()
        {
            Configure("webhooks", cfg =>
            {
                cfg.AllowAnonymous();
                cfg.Description(x =>
                    x.WithTags("Webhooks")
                    .Produces(StatusCodes.Status200OK)
                    .Produces(StatusCodes.Status401Unauthorized)
                    .Produces(StatusCodes.Status400BadRequest)
                    .Produces(StatusCodes.Status500InternalServerError));
            });
        }
    }


}
