using Minerals.StringCases;
using Nexa.BuildingBlocks.Domain.Exceptions;

namespace Nexa.CustomerManagement.Shared.Consts
{
    public class CustomerErrorConsts
    {
        public static NexaError CustomerNotExist =
            new(nameof(CustomerNotExist).ToLower(), "Current customer is not exist.");

        public static NexaError CustomerNotVerified =
            new(nameof(CustomerNotVerified).ToLower(), "The customer has not been verified yet.");

        public static NexaError UserAlreadyHasCustomer
            = new(nameof(UserAlreadyHasCustomer).ToCamelCase(), "User has already created customer entity.");

        public static NexaError IncompleteCustomerInfo
            = new(nameof(IncompleteCustomerInfo).ToCamelCase(), "Customer info must be completed first.");

        public static NexaError DocumentNotExist =
            new(nameof(DocumentNotExist).ToCamelCase(), "Customer dosen't provide document yet.");

        public static NexaError IncompleteDocument
            = new(nameof(IncompleteDocument).ToCamelCase(), "Document attachments is missing.");
    }
}
