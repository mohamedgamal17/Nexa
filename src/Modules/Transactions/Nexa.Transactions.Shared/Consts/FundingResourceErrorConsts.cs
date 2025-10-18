using Minerals.StringCases;
using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.Transactions.Shared.Consts
{
    public class FundingResourceErrorConsts
    {
        public static NexaError FundingResourceNotExist
            => new(nameof(FundingResourceNotExist).ToCamelCase(), "Funding resource is not exist.");

        public static NexaError FundingResourceNotOwned
            => new(nameof(FundingResourceNotOwned).ToCamelCase(), "The funding resource does not belong to the current user.");
    }
}
