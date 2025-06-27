using Bogus.Extensions.UnitedStates;
using FluentAssertions;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.CustomerApplications.CreateCustomerApplications;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Tests.CustomerApplications.Commands
{
    [TestFixture]
    public class CreateCustomerApplicationCommandHandletTests : CustomerApplicaitonTestFixture
    {
        [Test]
        public async Task Should_create_customer_application()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var command = PrepareCreateCustomerApplicationCommand();

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customerApplication = await CustomerApplicationRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id);

            customerApplication.Should().NotBeNull();

            customerApplication!.AssertCustomerApplication(command, fakeCustomer.Id);

            result.Value!.AssertCustomerApplicationDto(customerApplication!);
        }

        [Test]
        public async Task Should_failure_while_creating_customer_application_when_user_is_not_authorized()
        {
            var fakeCustomer = CreateCustomerAsync();

            var command = PrepareCreateCustomerApplicationCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }

        [Test]
        public async Task Should_failure_while_creating_customer_application_when_customer_is_not_created_yet()
        {
            AuthenticationService.Login();

            var command = PrepareCreateCustomerApplicationCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [Test]
        public async Task Should_failure_while_creating_customer_application_when_there_is_already_active_application()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer =  await CreateCustomerAsync(userId);

            await CreateCustomerApplicationAsync(fakeCustomer.Id);

            var command = PrepareCreateCustomerApplicationCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }



        private CreateCustomerApplicationCommand PrepareCreateCustomerApplicationCommand()
        {
            var command = new CreateCustomerApplicationCommand
            {
                FirstName = Faker.Person.FirstName,
                LastName = Faker.Person.LastName,
                PhoneNumber = Faker.Person.Phone,
                EmailAddress = Faker.Person.Email,
                Nationality = "US",
                SSN = Faker.Person.Ssn(),
                Gender = Faker.Person.Gender == Bogus.DataSets.Name.Gender.Male ? Gender.Male : Gender.Female,
                BirthDate = Faker.Person.DateOfBirth,
                Address = new AddressModel
                {
                    Country = "US",
                    City = Faker.Person.Address.City,
                    State = Faker.Person.Address.State,
                    StreetLine1 = Faker.Person.Address.Street,
                    StreetLine2 = Faker.Person.Address.Street,
                    PostalCode = Faker.Person.Address.ZipCode,
                    ZipCode = Faker.Person.Address.ZipCode
                }

            };

            return command;
        }
    }
}
