using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.Customers
{
    public class CustomerInfo : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Gender Gender { get; set; }
        public string Nationality { get; private set; }
        public string IdNumber { get; private set; }
        public Address Address { get; private set; }
        public string? KycReviewId { get; private set; }
        public VerificationState State { get; private set; }

        private CustomerInfo()
        {
            
        }
        public CustomerInfo(string firstName, string lastName, DateTime birthDate, string nationality, Gender gender, string idNumber, Address address)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;
            Nationality = nationality;
            IdNumber = idNumber;
            Address = address;
        }

        public void MarkAsProcessing(string kycReviewId)
        {
            if(State == VerificationState.Rejected 
                || State == VerificationState.Pending)
            {
                KycReviewId = kycReviewId;
                State = VerificationState.Processing;
            }
        }

        public void MarkAsVerified(string kycReviewId)
        {
            if(State == VerificationState.Processing)
            {
                KycReviewId = kycReviewId;
                State = VerificationState.Verified;
            }
        }

        public void MarkAsRejected(string kycReviewId)
        {
            if(State != VerificationState.Rejected)
            {
                KycReviewId = kycReviewId;
                State = VerificationState.Rejected;
            }          
        }

        public static CustomerInfo Create(string firstName, string lastName, DateTime birthDate, string nationality,  Gender gender, string idNumber, Address address)
        {
            return new CustomerInfo(firstName, lastName, birthDate, nationality, gender, idNumber, address);
        }
    }
}
