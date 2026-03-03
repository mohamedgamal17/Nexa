using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomeEmailByUserId;
using Nexa.Application.Tests.Extensions;
using FluentAssertions;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Application.Tests.Assertions;

namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    public class UpdateCustomerEmailByUserIdCommandHandlerTests : CustomerTestFixture
    {
        public ICustomerManagementRepository<Customer> CustomerRepository { get; }
        public UpdateCustomerEmailByUserIdCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [Test]
        public async Task Should_update_customer_email()
        {
            var userId = Guid.NewGuid().ToString();

            var fakeCustomer = await CreateCustomerAsync(userId);

            var command = new UpdateCustomerEmailByUserIdCommand
            {
                UserId = userId,
                EmailAddress =Faker.Person.Email
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            customer.EmailAddress.Should().Be(command.EmailAddress);

            result.Value!.AssertCustomerDto(customer);
        }

        [Test]
        public async Task Should_failure_while_updating_customer_email_when_customer_is_not_exist_for_current_user_id()
        {
            var userId = Guid.NewGuid().ToString();

            var command = new UpdateCustomerEmailByUserIdCommand
            {
                UserId = userId,
                EmailAddress = Faker.Person.Email
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException),CustomerErrorConsts.CustomerNotExist);
        }

    }
}
