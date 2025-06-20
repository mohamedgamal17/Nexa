using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCClientRequest
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
