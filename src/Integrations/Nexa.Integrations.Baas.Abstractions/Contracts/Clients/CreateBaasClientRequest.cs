namespace Nexa.Integrations.Baas.Abstractions.Contracts.Clients
{
    public class CreateBaasClientRequest
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SSN { get; set; }
        public Gender Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Address Address { get; set; }
    }
}
