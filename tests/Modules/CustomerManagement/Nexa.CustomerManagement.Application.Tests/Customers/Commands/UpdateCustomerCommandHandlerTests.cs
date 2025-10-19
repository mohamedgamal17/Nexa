using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Bogus;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomer;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    [TestFixture]

    public class UpdateCustomerCommandHandlerTests :  CustomerTestFixture
    {
        public ICustomerManagementRepository<Customer> CustomerRepository { get; }
        public UpdateCustomerCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [Test]
        public async Task Should_update_customer()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var command = PrepareUpdateCustomerCommand();

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            customer!.AssertCustomer(AuthenticationService.GetCurrentUser()!.Id, command);

            result.Value!.AssertCustomerDto(customer!);
        }

        [Test]
        public async Task Should_failure_while_updating_customer_when_user_is_not_authorized()
        {
            var command = PrepareUpdateCustomerCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(NexaUnauthorizedAccessException), GlobalErrorConsts.UnauthorizedAccess);
        }


        [Test]
        public async Task Should_failure_while_updating_when_current_user_does_not_have_customer_yet()
        {
            AuthenticationService.Login();

            var command = PrepareUpdateCustomerCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), CustomerErrorConsts.CustomerNotExist);

        }

        private UpdateCustomerCommand PrepareUpdateCustomerCommand()
        {
            var faker = new Faker();

            var command = new UpdateCustomerCommand()
            {
                FirstName = faker.Person.FirstName,
                LastName = faker.Person.LastName,
                Gender = faker.PickRandom<Gender>(),
                BirthDate = DateTime.Now.AddYears(-25),
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
