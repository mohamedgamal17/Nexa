using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Dtos
{
    public class AddressDto : EntityDto
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
