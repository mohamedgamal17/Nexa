using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Shared.Dtos
{
    public class CustomerApplicationDto : EntityDto
    {
        public string CustomerId { get; set; }
        public string KycExternalId { get; set; }
        public string? KycCheckId { get; set; }
        public string? CustomerApplicationExternalId { get; set; }
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
        public AddressDto Address { get; set; }
        public DateTime BirthDate { get; set; }
        public string Nationality { get; set; }
        public Gender Gender { get; set; }
        public string? SSN { get; set; }
        public string? NationalIdentityNumber { get; set; }
        public CustomerApplicationStatus Status { get;  set; }
    }
}
