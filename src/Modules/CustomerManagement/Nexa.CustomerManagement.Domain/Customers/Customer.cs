using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Domain.Customers.Events;
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
        public Document? Document { get; set; }
        public VerificationState State { get; private set; }

        public bool CanBeReviewed => Info?.State == VerificationState.Verified &&
            Document?.State == VerificationState.Verified;
        private Customer()
        {
            
        }

        public Customer(string userId , string phoneNumber, string emailAddres)
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddres;
        }

  
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
      
        public void Review(string? fintechCustomerid = null)
        {
            if (!CanBeReviewed)
            {
                throw new InvalidOperationException("Both of customer info and document should be verified first.");
            }

            if (fintechCustomerid != null)
            {
                FintechCustomerId = fintechCustomerid;
            }

            State = VerificationState.Processing;
        }

        public void Reject()
        {
            if(State == VerificationState.Processing)
            {
                State = VerificationState.Rejected;

                Info!.MarkAsRejected();

                Document!.MarkAsRejected();
            }   
        }

        public void Accept()
        {
            if(State == VerificationState.Processing)
            {
                State = VerificationState.Verified;
            }
        }

        public void ReviewCustomerInfo(KycReview kycReview)
        {
            if(Info != null)
            {
                Info.MarkAsProcessing(kycReview.Id);
            }
        }

        public void AcceptCustomerInfo()
        {
            if(Info != null)
            {
                Info.MarkAsVerified();

                var infoEvent = new CustomerInfoAcceptedEvent(Id);

                AppendEvent(infoEvent);
            }
        }

        public void RejectCustomerInfo()
        {
            if(Info != null)
            {
                Info.MarkAsRejected();

                if(Document != null && Document.State == VerificationState.Verified)
                {
                    RejectDocument();

                }

            }
        }

        public void AcceptDocument()
        {
            if(Document != null)
            {
                if(Info!.State == VerificationState.Rejected)
                {
                    RejectDocument();
                }
                else
                {
                    Document.MarkAsVerified();

                    var documentEvent = new CustomerInfoAcceptedEvent(Id);

                    AppendEvent(documentEvent);
                }
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
