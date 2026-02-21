using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Customers.Models;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerAddress;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerInfo;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Consts;

namespace Nexa.CustomerManagement.Application.Tests.OnboardCustomers.Commands
{
    public class UpdateOnboardCustomerAddressCommandHandlerTests : OnboardCustomerTestFixture
    {
        protected ICustomerManagementRepository<OnboardCustomer> OnboardCustomerRepository { get;  }

        public UpdateOnboardCustomerAddressCommandHandlerTests()
        {
            OnboardCustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<OnboardCustomer>>();
        }

        [Test]
        public async Task Should_update_onboard_customer_address()
        {
            string userId = Guid.NewGuid().ToString();

            var fakeOnboardCustomer = await CreateInitialOnboardCustomerAsync(userId);

            var command = new UpdateOnboardCustomerAddressCommand
            {
                UserId = userId,
                Address = new AddressModel
                {
                    Country = "US",
                    State = "CA",
                    City = "SAN",
                    StreetLine = "12 CA US",
                    PostalCode = "65454",
                    ZipCode = "4515"
                }
            };

            var result = await Mediator.Send(command);

            var onboardCustomer = await OnboardCustomerRepository.SingleAsync(x => x.Id == fakeOnboardCustomer.Id);

            onboardCustomer.Address.Should().NotBeNull();

            onboardCustomer.Address!.AssertAddress(command.Address);
        }

        [Test]
        public async Task Should_failure_while_updating_onboard_customer_address_when_onboard_customer_is_not_exist()
        {
            string userId = Guid.NewGuid().ToString();
            var command = new UpdateOnboardCustomerAddressCommand
            {
                UserId = userId,
                Address = new AddressModel
                {
                    Country = "US",
                    State = "CA",
                    City = "SAN",
                    StreetLine = "12 CA US",
                    PostalCode = "65454",
                    ZipCode = "4515"
                }
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), OnboardCustomerErrorConsts.OnboardCustomerNotExist);
        }

        [Test]
        public async Task Should_failure_while_updating_onboard_customer_address_when_onboard_customer_is_complete()
        {
            string userId = Guid.NewGuid().ToString();

            await CreateCompletedOnboardCustomer(userId);

            var command = new UpdateOnboardCustomerAddressCommand
            {
                UserId = userId,
                Address = new AddressModel
                {
                    Country = "US",
                    State = "CA",
                    City = "SAN",
                    StreetLine = "12 CA US",
                    PostalCode = "65454",
                    ZipCode = "4515"
                }
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException), OnboardCustomerErrorConsts.OnboardCustomerCompleted);
        }
    }
}
