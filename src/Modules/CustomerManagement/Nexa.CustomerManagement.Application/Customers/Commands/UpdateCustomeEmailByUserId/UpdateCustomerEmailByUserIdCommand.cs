using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomeEmailByUserId
{
    public class UpdateCustomerEmailByUserIdCommand : ICommand<CustomerDto>
    {
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}
