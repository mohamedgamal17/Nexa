using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
using System.Net;
namespace Nexa.CustomerManagement.Shared.Dtos
{
    public class CustomerDto : EntityDto
    {
        public string UserId { get; set; }
        public string? KycCustomerId { get; set; }
        public string? FintechCustomerId { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public CustomerInfoDto Info { get; set; }
        public DocumentDto Document { get; set; }
        public CustomerStatus Status { get; set; }
    }
}
