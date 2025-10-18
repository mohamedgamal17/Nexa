using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.BuildingBlocks.Domain.Consts
{
    public static class GlobalErrorConsts
    {
        public static NexaError ResourceNotFound
            => new("resoruceNotFound", "The specified {0} could not be found.");

        public static NexaError ForbiddenAccess
            => new("forbiddenAccess", "You don't have permission to access this resource.");

        public static NexaError UnauthorizedAccess
            = new("unauthorizedAccess", "You are not authorized to access this resource.");
    }
}
