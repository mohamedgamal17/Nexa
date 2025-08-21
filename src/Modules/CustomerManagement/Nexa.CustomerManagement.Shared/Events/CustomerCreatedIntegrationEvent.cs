namespace Nexa.CustomerManagement.Shared.Events
{
    public class CustomerCreatedIntegrationEvent
    {
        public string CustomerId { get; set; }
        public string UserId { get; set; }
        public string EmailAddress { get; set; }
        public string PhoneNumber { get; set; }
    }
}
