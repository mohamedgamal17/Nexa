using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
namespace Nexa.BuildingBlocks.Infrastructure.Extensions
{
    public static class EndpointResultsExtensions
    {
        public static IResult ValidationFailure(this ValidationResult result)
        {
            if (result.IsValid)
            {
                throw new InvalidOperationException("Validation result must be invalid to send bad reqeust");
            }

            var validationErrors = new ValidationProblemDetails(result.ToDictionary());
            return Results.BadRequest(validationErrors);
        }
        public static IResult ToOk<T>(this Result<T> result)
        {
            if (result.IsFailure)
            {
                return HandleFailureResults(result);
            }

            return Results.Ok(result.Value);
        }

        public static IResult ToCreated<T>(this Result<T> result, string? uri = null)
        {
            if (result.IsFailure)
            {
                return HandleFailureResults(result);

            }

            return Results.Created(uri, result.Value);
        }

        public static IResult ToCreatedAtRoute<T>(this Result<T> result, string? routeName = null, object? routeValues = null)
        {
            if (result.IsFailure)
            {
                return HandleFailureResults(result);

            }

            return Results.CreatedAtRoute(routeName: routeName, routeValues: routeValues, result.Value);
        }

        public static IResult ToNoContent<T>(this Result<T> result)
        {
            if (result.IsFailure)
            {
                return HandleFailureResults(result);
            }

            return Results.NoContent();
        }

   
        private static IResult HandleFailureResults<T>(Result<T> result)
        {
            Exception exception = result.Exception!;

            if (exception is ForbiddenAccessException forbiddenAccessException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status403Forbidden,
                    Title = forbiddenAccessException.Code,
                    Detail = forbiddenAccessException.Message,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3"
                };

                return Results.Problem(problemDetails);
            }
            else if (exception is UnauthorizedAccessException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = "Unauthorized",
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                };

                return Results.Problem(problemDetails);
            }
            else if (exception is NexaUnauthorizedAccessException  unauthorizedAccess)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status401Unauthorized,
                    Title = unauthorizedAccess.Code,
                    Detail = unauthorizedAccess.Message,
                    Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
                };

                return Results.Problem(problemDetails);
            }
            else if (exception is EntityNotFoundException notFoundException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status404NotFound,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = notFoundException.Code,
                    Detail = notFoundException.Message
                };

                return Results.Problem(problemDetails);
            }
            else if (exception is BusinessLogicException businessLogicException)
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status400BadRequest,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = businessLogicException.Code,
                    Detail = businessLogicException.Message
                };

                return Results.Problem(problemDetails);
            }
            else
            {
                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    Title = "Internal server error",
                    Detail = exception.Message
                };


                return Results.Problem(problemDetails);
            }
        }

    }
}
