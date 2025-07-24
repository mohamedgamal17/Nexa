using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCClient
    {
        public string Id { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public KYCClientInfo Info { get; set; }

    }

    public class KYCClientInfo
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nationality { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
        public string SSN { get; set; }
        public Address Address { get; set; }
    }
}
