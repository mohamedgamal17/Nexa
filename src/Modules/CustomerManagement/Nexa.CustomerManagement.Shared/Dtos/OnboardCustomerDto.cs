using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Shared.Dtos
{
    public class OnboardCustomerDto : EntityDto
    {
        public string  UserId { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public CustomerInfoDto? Info { get; set; }
        public OnboardCustomerStatus Status { get; set; }
    }
}
