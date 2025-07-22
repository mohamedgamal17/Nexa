using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.KYC
{
    public class KYCClientRequest
    {  
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }      
    }

   
}
