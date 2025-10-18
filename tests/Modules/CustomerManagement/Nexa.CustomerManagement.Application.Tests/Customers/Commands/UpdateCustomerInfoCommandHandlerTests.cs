using Bogus.Extensions.UnitedStates;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerInfo;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    [TestFixture]
    public class UpdateCustomerInfoCommandHandlerTests : CustomerTestFixture
    {
        protected ICustomerManagementRepository<Customer> CustomerRepository { get; }

        public UpdateCustomerInfoCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }


        [Test]
        public async Task Should_update_customer_info()
        {
            AuthenticationService.Login();

            var fakeCustomer = await CreateCustomerAsync(AuthenticationService.GetCurrentUser()!.Id);

            var command = PrepareUpdateCustomerInfoCommand();

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            customer.AssertCustomerInfo(command);

            result.Value!.AssertCustomerDto(customer);
        }

        [Test]
        public async Task Should_failure_while_updating_customer_info_when_user_is_not_authorized()
        {
            var command = PrepareUpdateCustomerInfoCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(NexaUnauthorizedAccessException), GlobalErrorConsts.UnauthorizedAccess);
        }

        [Test]
        public async Task Should_failure_while_updating_customer_info_when_customer_is_not_created_yet()
        {
            AuthenticationService.Login();

            var command = PrepareUpdateCustomerInfoCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException),CustomerErrorConsts.CustomerNotExist);
        }

        private UpdateCustomerInfoCommand PrepareUpdateCustomerInfoCommand()
        {
            var command = new UpdateCustomerInfoCommand
            {
                FirstName = Faker.Person.FirstName,
                LastName = Faker.Person.LastName,
                Nationality = "US",
                BirthDate = Faker.Person.DateOfBirth,
                IdNumber = Faker.Person.Ssn(),
                Gender = Faker.PickRandom<Gender>(),
                Address = new AddressModel
                {
                    Country = "US",
                    City = Faker.Person.Address.City,
                    State = Faker.Person.Address.State,
                    StreetLine = Faker.Person.Address.Street,
                    ZipCode = Faker.Person.Address.ZipCode
                }

            };

            return command;
        }
    }
}
