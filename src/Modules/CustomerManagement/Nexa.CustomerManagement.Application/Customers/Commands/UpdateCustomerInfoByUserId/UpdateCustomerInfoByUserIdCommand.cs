using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerInfoByUserId
{
    public class UpdateCustomerInfoByUserIdCommand : ICommand<CustomerDto>
    {
        public string UserId { get; set; }
        public CustomerInfoModel Info { get; set; }
    }
}
