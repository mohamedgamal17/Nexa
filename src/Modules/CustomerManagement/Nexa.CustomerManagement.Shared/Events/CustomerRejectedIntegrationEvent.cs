namespace Nexa.CustomerManagement.Shared.Events
{
    public class CustomerRejectedIntegrationEvent
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string FintechCustomerId { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
