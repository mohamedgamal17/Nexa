using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerPhoneByUserId
{
    public class UpdateCustomerPhoneByUserIdCommand : ICommand<CustomerDto>
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
    }
}
