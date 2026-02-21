using Nexa.BuildingBlocks.Domain;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.Customers
{
    public class CustomerInfo : BaseEntity
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public DateTime BirthDate { get; private set; }
        public Gender Gender { get; set; }

        private CustomerInfo()
        {
            
        }

        public CustomerInfo(string firstName, string lastName, DateTime birthDate, Gender gender)
        {
            FirstName = firstName;
            LastName = lastName;
            BirthDate = birthDate;
            Gender = gender;

        }
        public static CustomerInfo Create(string firstName, string lastName, DateTime birthDate, Gender gender)
        {
            return new CustomerInfo(firstName, lastName, birthDate, gender);
        }

    }
}
