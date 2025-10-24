namespace Nexa.Integrations.Baas.Abstractions.Contracts.Customers
{
    public class UpdateBaasCustomerRequest
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
