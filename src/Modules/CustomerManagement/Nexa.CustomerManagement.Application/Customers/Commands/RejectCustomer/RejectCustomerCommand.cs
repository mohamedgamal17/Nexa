using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.CustomerManagement.Application.Customers.Commands.RejectCustomer
{
    public class RejectCustomerCommand : ICommand
    {
        public string FintechCustomerId { get; set; }

    }
}
