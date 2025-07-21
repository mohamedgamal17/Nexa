using Nexa.BuildingBlocks.Domain;

namespace Nexa.CustomerManagement.Domain.Customers
{
    public class Address : ValueObject
    {
        public string Country { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string StreetLine { get; private set; }
        public string? PostalCode { get; private set; }
        public string? ZipCode { get; private set; }

        private Address()
        {
            
        }
        public Address(string country, string city, string state, string streetLine, string? postalCode = null, string? zipCode = null)
        {
            Country = country;
            City = city;
            State = state;
            StreetLine = streetLine;
            PostalCode = postalCode;
            ZipCode = zipCode;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Country;
            yield return City;
            yield return State;
            yield return StreetLine;
            yield return PostalCode;
            yield return ZipCode;
        }

        public static Address Create(string country, string city, string state, string streetLine, string? postalCode = null, string? zipCode = null)
        {
            return new Address(country, city, state, streetLine, postalCode, zipCode);
        }
    }
}
