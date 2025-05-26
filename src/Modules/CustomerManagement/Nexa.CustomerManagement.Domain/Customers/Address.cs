using Nexa.BuildingBlocks.Domain;

namespace Nexa.CustomerManagement.Domain.Customers
{
    public class Address : BaseEntity
    {
        public string Country { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string StreetLine1 { get; set; }
        public string StreetLine2 { get; set; }
        public string PostalCode { get; set; }
        public string ZipCode { get; set; }
    }
}
