using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Commands.VerifyCustomerInfo
{
    [Authorize]
    public class VerifyCustomerInfoCommand : ICommand<CustomerDto>
    {

    }

}
