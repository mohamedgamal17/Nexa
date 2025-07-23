using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.CustomerManagement.Application.Customers.Commands.AcceptCustomerInfo
{
    public class AcceptCustomerInfoCommand : ICommand
    {
        public string KycCustomerId { get; set; }
    }
}
