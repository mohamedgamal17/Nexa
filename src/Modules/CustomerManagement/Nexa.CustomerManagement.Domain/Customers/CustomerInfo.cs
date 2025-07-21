using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.Customers
{
    public class CustomerInfo : ValueObject
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Gender Gender { get; set; }
        public string Nationality { get; private set; }
        public string IdNumber { get; private set; }
        public Address Address { get; private set; }

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

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return FirstName;
            yield return LastName;
            yield return BirthDate;
            yield return Nationality;
            yield return IdNumber;
            yield return Address;
        }

        public static CustomerInfo Create(string firstName, string lastName, DateTime birthDate, string nationality,  Gender gender, string idNumber, Address address)
        {
            return new CustomerInfo(firstName, lastName, birthDate, nationality, gender, idNumber, address);
        }
    }
}
