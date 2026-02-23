using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
using System.Net.Mail;

namespace Nexa.CustomerManagement.Shared.Dtos
{
    public class OnboardCustomerDto : EntityDto
    {
        public string  UserId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public CustomerInfoDto? Info { get; set; }
        public AddressDto Address { get; set; }
        public bool EmailAddressProvided { get; set; }
        public bool PhoneNumberProvided { get; set; }
        public bool CustomerInfoProvided { get; set; }
        public bool AddressProvided { get; set; }
        public OnboardCustomerStatus Status { get; set; }
    }
}
