using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Domain.Customers
{
    public class Customer : AggregateRoot
    {
        public string UserId { get; set; }
        public string? KycCustomerId { get; set; }
        public string? FintechCustomerId { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public CustomerInfo? Info { get; set; }
        public Address? Address { get; set; }
        public Document? Document { get; set; }
        public CustomerStatus Status { get; private set; }

        private Customer()
        {
            
        }

        public Customer(string userId , string phoneNumber, string emailAddres)
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddres;
        }


        public void AddFintechCustomerId(string fintechCustomerId) => FintechCustomerId = fintechCustomerId;
        public void AddKycCustomerId(string kycCustomerId)
        {
            KycCustomerId = kycCustomerId;
        }

        public void Update(string phoneNumber , string emailAddres)
        {
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddres;
        }

        public void UpdateDocument(Document document)
        {
            Document = document;
        }

        public void UpdateInfo(CustomerInfo info)
        {
            Info = info;
        }
      
        public void UpdateAddress(Address address)
        {
            Address = address;
        }

        public void MarkAsVerified()
        {
            if (Status != CustomerStatus.Unverified)
            {
                return;
            }
            if (Document!.Status == DocumentVerificationStatus.Verified)
            {
                Status = CustomerStatus.Verified;
            }
        }
        public void AcceptDocument()
        {
            if(Document != null)
            {
                Document.MarkAsVerified();

                MarkAsVerified();
            }
        }

        public void RejectDocument()
        {
            if(Document != null)
            {
                Document.MarkAsRejected();
            }
        }
        public void ReviewDocument(KycReview kycReview)
        {
            if(Document  != null)
            {
                Document.MarkAsProcessing(kycReview.Id);
            }
        }
        public static Customer Create(string userId, string phoneNumber, string emailAddres)
        {
            return new Customer(userId, phoneNumber, emailAddres);
        }
    }
}
