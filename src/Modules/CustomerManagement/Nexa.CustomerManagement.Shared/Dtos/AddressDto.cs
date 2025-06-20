using Nexa.BuildingBlocks.Domain.Dtos;

namespace Nexa.CustomerManagement.Shared.Dtos
{
    public class AddressDto
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
