using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Queries.GetCurrentUserCustomer
{
    [Authorize]
    public class GetCurrentUserCustomerQuery : IQuery<CustomerDto>
    {

    }
}
