using Nexa.BuildingBlocks.Domain.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Shared.Dtos
{
    public class CustomerInfoDto 
    {
        public string FirstName { get;  set; }
        public string LastName { get;  set; }
        public DateTime BirthDate { get;  set; }
        public Gender Gender { get; set; }
        public AddressDto Address { get;  set; }
    }

    public class CustomerInfoPublicDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public Gender Gender { get; set; }
    }
}
