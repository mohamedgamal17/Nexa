using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.Customers
{
    public class Customer : BaseEntity
    {
        public string UserId { get; set; }
        public string? KycCustomerId { get; set; }
        public string? KycFintechCustomerid { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public CustomerInfo Info { get; set; }
        public VerificationState InfoVerificationState { get; set; }
        public List<Document> Documents { get; set; } = new List<Document>();
        public bool IsVerified => InfoVerificationState == VerificationState.Verified
            && Documents.All(x => x.State == VerificationState.Verified);


    }
}
