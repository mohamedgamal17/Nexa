using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerAddressByUserId;
using Nexa.Application.Tests.Extensions;

namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    public class UpdateCustomerAddressByUserIdCommandHandlerTests : CustomerTestFixture
    {
        public ICustomerManagementRepository<Customer> CustomerRepository { get; }
        public UpdateCustomerAddressByUserIdCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [Test]
        public async Task Should_update_customer_address()
        {
            var userId = Guid.NewGuid().ToString();

            var fakeCustomer = await CreateCustomerAsync(userId);

            var command = new UpdateCustomerAddressByUserIdCommand
            {
                UserId = userId,
                Addres = new AddressModel
                {
                    Country = "US",
                    State = "CA",
                    City = "Arizona",
                    StreetLine = "312 Arizona street",
                    PostalCode = "65554",
                    ZipCode = "45111"
                }

            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            customer.Address.Should().NotBeNull();

            customer.Address!.AssertAddress(command.Addres);

            result.Value!.AssertCustomerDto(customer);
        }

        [Test]
        public async Task Should_failure_while_updating_customer_address_when_customer_is_not_exist_for_current_user_id()
        {
            var userId = Guid.NewGuid().ToString();

            var command = new UpdateCustomerAddressByUserIdCommand
            {
                UserId = userId,
                Addres = new AddressModel
                {
                    Country = "US",
                    State = "CA",
                    City = "Arizona",
                    StreetLine = "312 Arizona street",
                    PostalCode = "65554",
                    ZipCode = "45111"
                }
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), CustomerErrorConsts.CustomerNotExist);
        }

    }
}
