using FluentAssertions;
using Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomer;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.Tests.Assertions
{
    public static class CustomerAssertionsExtensions
    {

        public static void AssertCustomer(this  Customer customer ,string userId , CreateCustomerCommand command)
        {
            customer.UserId.Should().Be(userId);
            customer.FirstName.Should().Be(command.FirstName);
            customer.MiddleName.Should().Be(command.MiddleName);
            customer.LastName.Should().Be(command.LastName);
            customer.EmailAddress.Should().Be(command.EmailAddress);
            customer.Gender.Should().Be(command.Gender);
            customer.PhoneNumber.Should().Be(command.PhoneNumber);
            customer.Nationality.Should().Be(command.Nationality);
            customer.BirthDate.Should().Be(command.BirthDate);
            
            if(command.Address != null)
            {
                customer.Address.Should().NotBeNull();

                customer.Address.Country.Should().Be(command.Address.Country);
                customer.Address.State.Should().Be(command.Address.State);
                customer.Address.City.Should().Be(command.Address.City);
                customer.Address.StreetLine1.Should().Be(command.Address.StreetLine1);
                customer.Address.StreetLine2.Should().Be(command.Address.StreetLine2);
                customer.Address.PostalCode.Should().Be(command.Address.PostalCode);
                customer.Address.ZipCode.Should().Be(command.Address.ZipCode);
            }
        }

        public static void AssertCustomer(this  Customer customer, string userId, UpdateCustomerCommand command)
        {
            customer.UserId.Should().Be(userId);
            customer.FirstName.Should().Be(command.FirstName);
            customer.MiddleName.Should().Be(command.MiddleName);
            customer.LastName.Should().Be(command.LastName);
            customer.EmailAddress.Should().Be(command.EmailAddress);
            customer.Gender.Should().Be(command.Gender);
            customer.PhoneNumber.Should().Be(command.PhoneNumber);
            customer.Nationality.Should().Be(command.Nationality);
            customer.BirthDate.Should().Be(command.BirthDate);

            if (command.Address != null)
            {
                customer.Address.Should().NotBeNull();

                customer.Address.Country.Should().Be(command.Address.Country);
                customer.Address.State.Should().Be(command.Address.State);
                customer.Address.City.Should().Be(command.Address.City);
                customer.Address.StreetLine1.Should().Be(command.Address.StreetLine1);
                customer.Address.StreetLine2.Should().Be(command.Address.StreetLine2);
                customer.Address.PostalCode.Should().Be(command.Address.PostalCode);
                customer.Address.ZipCode.Should().Be(command.Address.ZipCode);
            }
        }

        public static void AssertCustomerDto(this CustomerDto dto , Customer customer)
        {
            dto.Id.Should().Be(customer.Id);
            dto.UserId.Should().Be(dto.UserId);
            dto.FirstName.Should().Be(customer.FirstName);
            dto.MiddleName.Should().Be(customer.MiddleName);
            dto.LastName.Should().Be(customer.LastName);
            dto.EmailAddress.Should().Be(customer.EmailAddress);
            dto.Gender.Should().Be(customer.Gender);
            dto.PhoneNumber.Should().Be(customer.PhoneNumber);
            dto.Nationality.Should().Be(customer.Nationality);
            dto.BirthDate.Should().Be(customer.BirthDate);

            if (customer.Address != null)
            {
                dto.Address.Should().NotBeNull();

                dto.Address.Country.Should().Be(customer.Address.Country);
                dto.Address.State.Should().Be(customer.Address.State);
                dto.Address.City.Should().Be(customer.Address.City);
                dto.Address.StreetLine1.Should().Be(customer.Address.StreetLine1);
                dto.Address.StreetLine2.Should().Be(customer.Address.StreetLine2);
                dto.Address.PostalCode.Should().Be(customer.Address.PostalCode);
                dto.Address.ZipCode.Should().Be(customer.Address.ZipCode);
            }
        }

    }
}
