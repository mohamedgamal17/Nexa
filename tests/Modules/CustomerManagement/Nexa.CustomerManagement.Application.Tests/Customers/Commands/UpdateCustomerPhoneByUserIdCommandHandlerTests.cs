using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomeEmailByUserId;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Shared.Consts;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerPhoneByUserId;
using Nexa.Application.Tests.Extensions;
using FluentAssertions;

namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    public class UpdateCustomerPhoneByUserIdCommandHandlerTests : CustomerTestFixture
    {

        public ICustomerManagementRepository<Customer> CustomerRepository { get; }
        public UpdateCustomerPhoneByUserIdCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [Test]
        public async Task Should_update_customer_phone()
        {
            var userId = Guid.NewGuid().ToString();

            var fakeCustomer = await CreateCustomerAsync(userId);

            var command = new UpdateCustomerPhoneByUserIdCommand
            {
                UserId = userId,
                PhoneNumber = Faker.Person.Phone
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            customer.PhoneNumber.Should().Be(command.PhoneNumber);
        }

        [Test]
        public async Task Should_failure_while_creating_customer_phone_when_customer_is_not_exist_for_current_user_id()
        {
            var userId = Guid.NewGuid().ToString();

            var command = new UpdateCustomerPhoneByUserIdCommand
            {
                UserId = userId,
                PhoneNumber = Faker.Person.Phone
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), CustomerErrorConsts.CustomerNotExist);
        }
    }
}
