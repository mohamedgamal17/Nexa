using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Domain.Documents;
namespace Nexa.CustomerManagement.Domain.Customers
{
    public class Customer : BaseEntity
    {
        public string UserId { get; set; }
        public string? KycCustomerId { get; set; }
        public string? FintechCustomerid { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public CustomerInfo? Info { get; set; }
        public Document Document { get; set; }

        private Customer()
        {
            
        }

        public Customer(string userId , string phoneNumber, string emailAddres)
        {
            UserId = userId;
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddres;
        }

  
        public void AddKycCustomerId(string kycCustomerId)
        {
            KycCustomerId = kycCustomerId;
        }

        public void Update(string phoneNumber , string emailAddres)
        {
            PhoneNumber = phoneNumber;
            EmailAddress = emailAddres;
        }

        public void UpdateDocument(Document document)
        {
            Document = document;
        }

        public void UpdateInfo(CustomerInfo info)
        {
            Info = info;
        }
      
        public static Customer Create(string userId, string phoneNumber, string emailAddres)
        {
            return new Customer(userId, phoneNumber, emailAddres);
        }
    }
}
