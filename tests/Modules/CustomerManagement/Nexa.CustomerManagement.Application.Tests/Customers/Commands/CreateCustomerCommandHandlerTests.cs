using Bogus;
using FluentAssertions;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Events;
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
            await TestHarness.Start();

            AuthenticationService.Login();

            var command = PrepareCreateCustomerCommand();

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var entity = await CustomerRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id);

            entity.Should().NotBeNull();

            entity!.AssertCustomer(AuthenticationService.GetCurrentUser()!.Id,command);

            result.Value!.AssertCustomerDto(entity!);

            Assert.That(await TestHarness.Published.Any<CustomerCreatedIntegrationEvent>());

            await TestHarness.Stop();

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
                EmailAddress = faker.Person.Email,
                PhoneNumber = faker.Person.Phone,
            };

            return command;
        }

    }
}
