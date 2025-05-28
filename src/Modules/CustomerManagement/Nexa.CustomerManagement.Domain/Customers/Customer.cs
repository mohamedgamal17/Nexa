using Nexa.BuildingBlocks.Domain;

namespace Nexa.CustomerManagement.Domain.Customers
{
    public class Customer : BaseEntity
    {
        public string UserId { get; set; }

        public string ExternalId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public Address Address { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public string? SocialInsuranceNumber { get; set; }
        public string? NationalIdentityNumber { get; set; }
        public string? TaxIdentificationNumber { get; set; }
    }
}
