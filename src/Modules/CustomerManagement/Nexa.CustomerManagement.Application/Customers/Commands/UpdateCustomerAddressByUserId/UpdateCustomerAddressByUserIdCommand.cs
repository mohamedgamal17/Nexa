using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerAddressByUserId
{
    public class UpdateCustomerAddressByUserIdCommand : ICommand<CustomerDto>
    {
        public string UserId { get; set; }
        public AddressModel  Addres { get; set; }
    }

}
