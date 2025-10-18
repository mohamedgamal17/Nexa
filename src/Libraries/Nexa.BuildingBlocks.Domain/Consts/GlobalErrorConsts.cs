using Minerals.StringCases;
using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.BuildingBlocks.Domain.Consts
{
    public static class GlobalErrorConsts
    {

        public static NexaError UnauthorizedAccess
            = new(nameof(UnauthorizedAccess).ToCamelCase(), "You are not authorized to access this resource.");
    }
}
