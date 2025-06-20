using Bogus;
using Bogus.Extensions.Canada;
using Bogus.Extensions.UnitedStates;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    [TestFixture]
    public class CreateCustomerCommandHandlerTests : CustomerTestFixture
    {
        public ICustomerManagementRepository<Customer> CustomerRepository { get; }

        public CreateCustomerCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [Test]
        public async Task Should_create_user_customer()
        {
            AuthenticationService.Login();

            var command = PrepareCreateCustomerCommand();

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var entity = await CustomerRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id);

            entity.Should().NotBeNull();

            entity!.AssertCustomer(AuthenticationService.GetCurrentUser()!.Id,command);

            result.Value!.AssertCustomerDto(entity!);
        }

        [Test]
        public async Task Should_failure_while_creating_user_customer_when_user_is_not_authorized()
        {
            var command = PrepareCreateCustomerCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }


        [Test]
        public async Task Should_failure_while_creating_user_customer_when_customer_is_already_created_for_current_user()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            await CreateCustomerAsync(userId);

            var command = PrepareCreateCustomerCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        private CreateCustomerCommand PrepareCreateCustomerCommand()
        {
            var faker = new Faker();

            var command = new CreateCustomerCommand()
            {
                FirstName = faker.Person.FirstName,
                MiddleName = faker.Person.LastName,
                LastName = faker.Person.LastName,
                Gender = faker.PickRandom<Gender>(),
                EmailAddress = faker.Person.Email,
                PhoneNumber = "13322767084",
                SocialSecurityNumber = faker.Person.Ssn(),
                Nationality = "US",
                BirthDate = DateTime.Now.AddYears(-25),
                Address = new AddressModel
                {
                    Country = faker.Address.CountryCode(),
                    State = faker.Address.State(),
                    City = faker.Address.City(),
                    StreetLine1 = faker.Address.StreetAddress(),
                    StreetLine2 = faker.Address.StreetAddress(),
                    PostalCode = faker.Address.ZipCode(),
                    ZipCode = faker.Address.ZipCode()
                },
            };

            return command;
        }

    }
}
