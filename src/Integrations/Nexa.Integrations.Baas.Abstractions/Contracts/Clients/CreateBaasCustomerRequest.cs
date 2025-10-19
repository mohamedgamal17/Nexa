namespace Nexa.Integrations.Baas.Abstractions.Contracts.Clients
{
    public class CreateBaasCustomerRequest
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
