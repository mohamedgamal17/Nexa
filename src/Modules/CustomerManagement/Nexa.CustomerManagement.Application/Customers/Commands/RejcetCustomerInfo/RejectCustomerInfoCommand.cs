using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.CustomerManagement.Application.Customers.Commands.RejcetCustomerInfo
{
    public class RejectCustomerInfoCommand : ICommand
    {
        public string KycCustomerId { get; set; }
    }
}
