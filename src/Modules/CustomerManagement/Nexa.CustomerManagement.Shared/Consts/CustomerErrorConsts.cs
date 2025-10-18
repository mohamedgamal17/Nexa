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

        public static NexaError InvalidCustomerInfoVerificationState
                 = new(nameof(InvalidCustomerInfoVerificationState).ToCamelCase(), "Invalid customer info verification state.");

        public static NexaError DocumentNotExist =
            new(nameof(DocumentNotExist).ToCamelCase(), "Customer dosen't provide document yet.");

        public static NexaError DocumentNotOwned
            = new(nameof(DocumentNotOwned).ToCamelCase(), "The document does not belong to the current user.");

        public static NexaError IncompleteDocument
            = new(nameof(IncompleteDocument).ToCamelCase(), "Document attachments is missing.");

        public static NexaError InvalidDocumentVerificationState 
            = new(nameof(InvalidDocumentVerificationState).ToCamelCase(), "Invalid document verification state.");
    }
}
