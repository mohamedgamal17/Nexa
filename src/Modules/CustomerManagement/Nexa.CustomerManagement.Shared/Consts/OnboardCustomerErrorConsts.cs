using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.CustomerManagement.Shared.Consts
{
    public class OnboardCustomerErrorConsts
    {
        public static NexaError OnboardCustomerCreateConflict =
            new NexaError(nameof(OnboardCustomerCreateConflict), "A customer already exists for the provided UserId. Onboarding cannot be started again.");
    }
}
