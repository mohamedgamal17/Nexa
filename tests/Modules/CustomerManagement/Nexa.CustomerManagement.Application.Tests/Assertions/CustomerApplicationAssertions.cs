using FluentAssertions;
using Nexa.CustomerManagement.Application.CustomerApplications.CreateCustomerApplications;
using Nexa.CustomerManagement.Domain.CustomerApplications;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Nexa.CustomerManagement.Application.Tests.Assertions
{
    public static class CustomerApplicationAssertions
    {
        public static void AssertCustomerApplication(this CustomerApplication application ,CreateCustomerApplicationCommand command , string customerId)
        {
            application.CustomerId.Should().Be(customerId);
            application.FirstName.Should().Be(command.FirstName);
            application.MiddleName.Should().Be(command.MiddleName);
            application.LastName.Should().Be(command.LastName);
            application.EmailAddress.Should().Be(command.EmailAddress);
            application.Gender.Should().Be(command.Gender);
            application.PhoneNumber.Should().Be(command.PhoneNumber);
            application.BirthDate.Should().Be(command.BirthDate);
            application.Nationality.Should().Be(command.Nationality);
           
            if(command.Address != null)
            {
                application.Address.Should().NotBeNull();

                application.Address.Country.Should().Be(command.Address.Country);
                application.Address.City.Should().Be(command.Address.City);
                application.Address.State.Should().Be(command.Address.State);
                application.Address.StreetLine1.Should().Be(command.Address.StreetLine1);
                application.Address.StreetLine2.Should().Be(command.Address.StreetLine2);
                application.Address.PostalCode.Should().Be(command.Address.PostalCode);
                application.Address.ZipCode.Should().Be(command.Address.ZipCode);
            }
        }

        public static void AssertCustomerApplicationDto(this CustomerApplicationDto dto , CustomerApplication application)
        {
            dto.Id.Should().Be(application.Id);
            dto.CustomerId.Should().Be(application.CustomerId);
            dto.KycExternalId.Should().Be(application.KycExternalId);
            dto.KycCheckId.Should().Be(application.KycCheckId);
            dto.CustomerId.Should().Be(application.CustomerId);
            dto.FirstName.Should().Be(application.FirstName);
            dto.MiddleName.Should().Be(application.MiddleName);
            dto.LastName.Should().Be(application.LastName);
            dto.EmailAddress.Should().Be(application.EmailAddress);
            dto.Gender.Should().Be(application.Gender);
            dto.PhoneNumber.Should().Be(application.PhoneNumber);
            dto.BirthDate.Should().Be(application.BirthDate);
            dto.Nationality.Should().Be(application.Nationality);

            if (application.Address != null)
            {
                dto.Address.Should().NotBeNull();

                dto.Address.Country.Should().Be(application.Address.Country);
                dto.Address.City.Should().Be(application.Address.City);
                dto.Address.State.Should().Be(application.Address.State);
                dto.Address.StreetLine1.Should().Be(application.Address.StreetLine1);
                dto.Address.StreetLine2.Should().Be(application.Address.StreetLine2);
                dto.Address.PostalCode.Should().Be(application.Address.PostalCode);
                dto.Address.ZipCode.Should().Be(application.Address.ZipCode);
            }
        }
    }
}
