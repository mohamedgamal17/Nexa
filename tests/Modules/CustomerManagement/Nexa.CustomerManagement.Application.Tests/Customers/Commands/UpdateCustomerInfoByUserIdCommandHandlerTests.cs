using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerPhoneByUserId;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Shared.Consts;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerInfoByUserId;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.Application.Tests.Extensions;
using FluentAssertions;
using Nexa.CustomerManagement.Application.Tests.Assertions;

namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    public class UpdateCustomerInfoByUserIdCommandHandlerTests : CustomerTestFixture
    {
        public ICustomerManagementRepository<Customer> CustomerRepository { get; }
        public UpdateCustomerInfoByUserIdCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [Test]
        public async Task Should_update_customer_info()
        {
            var userId = Guid.NewGuid().ToString();

            var fakeCustomer = await CreateCustomerAsync(userId);

            var command = new UpdateCustomerInfoByUserIdCommand
            {
                UserId = userId,
                Info = new CustomerInfoModel
                {
                    FirstName = Faker.Person.FirstName,
                    LastName = Faker.Person.LastName,
                    BirthDate = Faker.Person.DateOfBirth,
                    Gender = Faker.PickRandom<Gender>()
                }
   
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            customer.Info.Should().NotBeNull();

            customer.Info!.AssertCustomerInfo(command.Info);

            result.Value!.AssertCustomerDto(customer);
        }

        [Test]
        public async Task Should_failure_while_updating_customer_info_when_customer_is_not_exist_for_current_user_id()
        {
            var userId = Guid.NewGuid().ToString();

            var command = new UpdateCustomerInfoByUserIdCommand
            {
                UserId = userId,
                Info = new CustomerInfoModel
                {
                    FirstName = Faker.Person.FirstName,
                    LastName = Faker.Person.LastName,
                    BirthDate = Faker.Person.DateOfBirth,
                    Gender = Faker.PickRandom<Gender>()
                }

            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), CustomerErrorConsts.CustomerNotExist);
        }

    }
}
