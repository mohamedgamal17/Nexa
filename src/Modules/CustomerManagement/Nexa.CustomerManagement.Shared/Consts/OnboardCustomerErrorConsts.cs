using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.CustomerManagement.Shared.Consts
{
    public class OnboardCustomerErrorConsts
    {
        public static NexaError OnboardCustomerCreateConflict =
            new NexaError(nameof(OnboardCustomerCreateConflict), "A customer already exists for the provided UserId. Onboarding cannot be started again.");

        public static NexaError OnboardCustomerNotExist =
            new NexaError(nameof(OnboardCustomerNotExist), "onboard customer is not exist.");
    }
}
