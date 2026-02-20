using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.OnboardCustomers.Queries.GetOnboardCustomerByUserId
{
    public class GetOnboardCustomerByUserIdQuery : IQuery<OnboardCustomerDto>
    {
        public string UserId { get; set; }
    }

}
