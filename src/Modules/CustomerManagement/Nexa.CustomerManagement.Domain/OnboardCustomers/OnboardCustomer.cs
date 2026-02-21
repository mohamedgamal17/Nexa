using MediatR;
using Nexa.BuildingBlocks.Domain;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.OnboardCustomers.Events;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Domain.OnboardCustomers
{
    public class OnboardCustomer : AggregateRoot
    {
        public string UserId { get;private set; }
        public string? EmailAddress { get; private set; }
        public string? PhoneNumber { get; private set; }
        public CustomerInfo? Info { get; private set; }
        public Address? Address { get; set; }
        public OnboardCustomerStatus Status { get; private set; }
        public bool IsCompleted => Status == OnboardCustomerStatus.Completed;
        public bool EmailAddressProvided => EmailAddress != null;
        public bool PhoneNumberProvided => PhoneNumber != null;
        public bool CustomerInfoProvided => Info != null;
        public bool AddressProvided => Address != null;

        public bool HasFullData => EmailAddressProvided
                && PhoneNumberProvided
                && CustomerInfoProvided
                && AddressProvided;
        private OnboardCustomer()
        {

        }
        public OnboardCustomer(string userId)
        {
            UserId = userId;
        }

        public void UpdateEmailAddress(string emailAddress)
        {
            ThrowIfCompleted();

            EmailAddress = emailAddress;

        }

        public void UpdatePhoneNumber(string phoneNumber)
        {
            ThrowIfCompleted();

            PhoneNumber = phoneNumber;
        }

        public void UpdateCustomerInfo(CustomerInfo info)
        {
            ThrowIfCompleted();

            Info = info;

        }
        public void MarkAsCompleted()
        {
            if(Status == OnboardCustomerStatus.Pending && HasFullData)
            {
                var @event = new OnboardCustomerCompletedEvent(Id, UserId, EmailAddress!, PhoneNumber!, Info!);

                AppendEvent(@event);

                Status = OnboardCustomerStatus.Completed;
            }
        }

        private void ThrowIfCompleted()
        {
            if (IsCompleted)
            {
                throw new InvalidOperationException($"{nameof(OnboardCustomer)} current state is completed it should be in pending state to be able to modifiy it's data");
            }
        }
    }
}
