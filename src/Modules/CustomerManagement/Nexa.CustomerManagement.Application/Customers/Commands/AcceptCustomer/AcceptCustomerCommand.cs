using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.CustomerManagement.Application.Customers.Commands.AcceptCustomer
{
    public class AcceptCustomerCommand : ICommand
    {
        public string FintechCustomerId { get; set; }
    }
}
