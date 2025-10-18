using Minerals.StringCases;
using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.BuildingBlocks.Domain.Consts
{
    public static class GlobalErrorConsts
    {
        public static NexaError ResourceNotFound
            => new(nameof(ResourceNotFound).ToCamelCase(), "The specified resource could not be found.");
        public static NexaError ForbiddenAccess
            => new(nameof(ForbiddenAccess).ToCamelCase(), "You don't have permission to access this resource.");

        public static NexaError UnauthorizedAccess
            = new(nameof(UnauthorizedAccess).ToCamelCase(), "You are not authorized to access this resource.");
    }
}
