using FluentAssertions;
using Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomer;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.Tests.Assertions
{
    public static class CustomerAssertionsExtensions
    {

        public static void AssertCustomer(this  Customer customer ,string userId , CreateCustomerCommand command)
        {
            customer.UserId.Should().Be(userId);
            customer.EmailAddress.Should().Be(command.EmailAddress);
            customer.PhoneNumber.Should().Be(command.PhoneNumber);
            customer.KycCustomerId.Should().NotBeNull();
            customer.FintechCustomerId.Should().NotBeNull();
            customer.Info.Should().NotBeNull();
            customer.Info!.FirstName.Should().Be(command.FirstName);
            customer.Info!.LastName.Should().Be(command.LastName);
            customer.Info.Gender.Should().Be(command.Gender);
            customer.Info.BirthDate.Should().Be(command.BirthDate);

            if(command.Address != null)
            {
                customer.Address.Should().NotBeNull();
                customer.Address!.AssertAddress(command.Address);
            }
        }

        public static void AssertCustomer(this  Customer customer, string userId, UpdateCustomerCommand command)
        {
            customer.UserId.Should().Be(userId);
            customer.Info.Should().NotBeNull();
            customer.Info!.FirstName.Should().Be(command.FirstName);
            customer.Info!.LastName.Should().Be(command.LastName);
            customer.Info.Gender.Should().Be(command.Gender);
            customer.Info.BirthDate.Should().Be(command.BirthDate);
            if (command.Address != null)
            {
                customer.Address.Should().NotBeNull();
                customer.Address!.AssertAddress(command.Address);
            }
        }
        public static void AssertCustomerToOnboard(this Customer customer , OnboardCustomer onboardCustomer)
        {
            customer.UserId.Should().Be(onboardCustomer.UserId);
            customer.PhoneNumber.Should().Be(onboardCustomer.PhoneNumber);
            customer.EmailAddress.Should().Be(onboardCustomer.EmailAddress);
            customer.UserId.Should().Be(onboardCustomer.UserId);
            customer.Info.Should().Be(onboardCustomer.Info);
            customer.Address.Should().Be(onboardCustomer.Address);
        }

        public static void AssertAddress(this Address address , AddressModel model)
        {
            address.Country.Should().Be(model.Country);
            address.City.Should().Be(model.City);
            address.State.Should().Be(model.State);
            address.StreetLine.Should().Be(model.StreetLine);
            address.PostalCode.Should().Be(model.PostalCode);
            address.ZipCode.Should().Be(model.ZipCode);
        }
        public static void AssertCustomerDto(this CustomerDto dto , Customer customer)
        {
            dto.Id.Should().Be(customer.Id);
            dto.UserId.Should().Be(dto.UserId);
            dto.EmailAddress.Should().Be(customer.EmailAddress);
            dto.PhoneNumber.Should().Be(customer.PhoneNumber);

            if(customer.Info != null)
            {
                dto.Info.AssertCustomerInfoDto(customer.Info);
            }

            if(customer.Address != null)
            {
                dto.Address.AssertAddressDto(customer.Address);
            }

            if(customer.Document != null)
            {
                dto.Document.AssertDocumentDto(customer.Document);
            }

        }

        public static void AssertCustomerInfoDto(this CustomerInfoDto dto , CustomerInfo info)
        {
            dto.FirstName.Should().Be(info.FirstName);
            dto.LastName.Should().Be(info.LastName);
            dto.Gender.Should().Be(info.Gender);
            dto.BirthDate.Should().Be(info.BirthDate);
           
        }
        public static void AssertAddressDto(this AddressDto dto , Address address)
        {
            dto.Country.Should().Be(address.Country);
            dto.City.Should().Be(address.City);
            dto.State.Should().Be(address.State);
            dto.StreetLine.Should().Be(address.StreetLine);
            dto.PostalCode.Should().Be(address.PostalCode);
            dto.ZipCode.Should().Be(address.ZipCode);
        }

    }
}
