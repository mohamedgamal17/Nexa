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
            customer.LastName.Should().Be(command.LastName);
            customer.EmailAddress.Should().Be(command.EmailAddress);
            customer.Gender.Should().Be(command.Gender);
            customer.PhoneNumber.Should().Be(command.PhoneNumber);
            customer.BirthDate.Should().Be(command.BirthDate);
                      
        }

        public static void AssertCustomer(this  Customer customer, string userId, UpdateCustomerCommand command)
        {
            customer.UserId.Should().Be(userId);
            customer.FirstName.Should().Be(command.FirstName);
            customer.LastName.Should().Be(command.LastName);
            customer.EmailAddress.Should().Be(command.EmailAddress);
            customer.Gender.Should().Be(command.Gender);
            customer.PhoneNumber.Should().Be(command.PhoneNumber);
            customer.BirthDate.Should().Be(command.BirthDate);
        }

        public static void AssertCustomerDto(this CustomerDto dto , Customer customer)
        {
            dto.Id.Should().Be(customer.Id);
            dto.UserId.Should().Be(dto.UserId);
            dto.FirstName.Should().Be(customer.FirstName);
            dto.LastName.Should().Be(customer.LastName);
            dto.EmailAddress.Should().Be(customer.EmailAddress);
            dto.Gender.Should().Be(customer.Gender);
            dto.PhoneNumber.Should().Be(customer.PhoneNumber);
            dto.BirthDate.Should().Be(customer.BirthDate);
        }

    }
}
