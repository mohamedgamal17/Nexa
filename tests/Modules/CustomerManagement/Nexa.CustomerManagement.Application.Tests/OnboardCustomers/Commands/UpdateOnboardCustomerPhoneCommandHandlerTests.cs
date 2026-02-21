using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerPhone;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Consts;

namespace Nexa.CustomerManagement.Application.Tests.OnboardCustomers.Commands
{
    public class UpdateOnboardCustomerPhoneCommandHandlerTests : OnboardCustomerTestFixture
    {
        public ICustomerManagementRepository<OnboardCustomer> OnboardCustomerRepository { get;  }

        public UpdateOnboardCustomerPhoneCommandHandlerTests()
        {
            OnboardCustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<OnboardCustomer>>();
        }

        [Test]
        public async Task Should_update_onboard_customer_phone()
        {
            string userId = Guid.NewGuid().ToString();

            var fakeOnboardCustomer = await CreateInitialOnboardCustomerAsync(userId);

            var command = new UpdateOnboardCustomerPhoneCommand
            {
                UserId = userId,
                PhoneNumber = Faker.Person.Phone
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var onboardCustomer = await OnboardCustomerRepository.SingleAsync(x => x.Id == fakeOnboardCustomer.Id);

            onboardCustomer.PhoneNumber.Should().Be(command.PhoneNumber);
        }

        [Test]
        public async Task Should_failure_while_updating_onboard_customer_phone_when_onboard_customer_is_not_exist()
        {
            string userId = Guid.NewGuid().ToString();

            var command = new UpdateOnboardCustomerPhoneCommand
            {
                UserId = userId,
                PhoneNumber = Faker.Person.Phone
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), OnboardCustomerErrorConsts.OnboardCustomerNotExist);
        }

        [Test]
        public async Task Should_failure_while_updating_customer_phone_whe_onboard_customer_is_in_complete_state()
        {
            string userId = Guid.NewGuid().ToString();

            await CreateCompletedOnboardCustomer(userId);

            var command = new UpdateOnboardCustomerPhoneCommand
            {
                UserId = userId,
                PhoneNumber = Faker.Person.Phone
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), OnboardCustomerErrorConsts.OnboardCustomerCompleted);
        }
    }
}
