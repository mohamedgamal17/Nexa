using FluentAssertions;
using Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomer;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerInfo;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.Tests.Assertions
{
    public static class CustomerAssertionsExtensions
    {

        public static void AssertCustomer(this  Customer customer ,string userId , CreateCustomerCommand command)
        {
            customer.UserId.Should().Be(userId);
            customer.KycCustomerId.Should().NotBeNull();
            customer.EmailAddress.Should().Be(command.EmailAddress);
            customer.PhoneNumber.Should().Be(command.PhoneNumber);
                      
        }

        public static void AssertCustomer(this  Customer customer, string userId, UpdateCustomerCommand command)
        {
            customer.UserId.Should().Be(userId);
            customer.EmailAddress.Should().Be(command.EmailAddress);
            customer.PhoneNumber.Should().Be(command.PhoneNumber);
        }

        public static void AssertCustomerInfo(this Customer customer  ,UpdateCustomerInfoCommand command)
        {
            customer.Info.Should().NotBeNull();
            customer.Info!.FirstName.Should().Be(command.FirstName);
            customer.Info.LastName.Should().Be(command.LastName);
            customer.Info.Gender.Should().Be(command.Gender);
            customer.Info.BirthDate.Should().Be(command.BirthDate);
            customer.Info.Nationality.Should().Be(command.Nationality);
            customer.Info.IdNumber.Should().Be(command.IdNumber);
            customer.Info.Address.AssertAddress(command.Address);
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

        }

        public static void AssertCustomerInfoDto(this CustomerInfoDto dto , CustomerInfo info)
        {
            dto.FirstName.Should().Be(info.FirstName);
            dto.LastName.Should().Be(info.LastName);
            dto.Gender.Should().Be(info.Gender);
            dto.BirthDate.Should().Be(info.BirthDate);
            dto.Nationality.Should().Be(info.Nationality);
            dto.IdNumber.Should().Be(info.IdNumber);
            dto.Address.AssertAddressDto(info.Address);
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
