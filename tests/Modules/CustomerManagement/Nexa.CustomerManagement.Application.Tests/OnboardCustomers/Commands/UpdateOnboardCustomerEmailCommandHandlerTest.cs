using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerEmail;
using Nexa.Application.Tests.Extensions;
using FluentAssertions;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Shared.Consts;

namespace Nexa.CustomerManagement.Application.Tests.OnboardCustomers.Commands
{
    [TestFixture]
    public class UpdateOnboardCustomerEmailCommandHandlerTest : OnboardCustomerTestFixture
    {
        public ICustomerManagementRepository<OnboardCustomer> OnboardCustomerRepository { get; }


        public UpdateOnboardCustomerEmailCommandHandlerTest()
        {
            OnboardCustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<OnboardCustomer>>();
        }

        [Test]
        public async Task Should_update_onboard_customer_email()
        {
            string userId = Guid.NewGuid().ToString();

            var fakeOnboardCustomer = await CreateInitialOnboardCustomerAsync(userId);

            var command = new UpdateOnboardCustomerEmailCommand
            {
                UserId = userId,
                EmailAddress = Faker.Person.Email
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var onboardCustomer = await OnboardCustomerRepository.SingleAsync(x => x.Id == fakeOnboardCustomer.Id);

            onboardCustomer.EmailAddress.Should().Be(command.EmailAddress);

        }

        [Test]
        public async Task Should_failure_while_updating_onboard_customer_when_onboard_customer_is_not_exist()
        {
            string userId = Guid.NewGuid().ToString();

            var command = new UpdateOnboardCustomerEmailCommand
            {
                UserId = userId,
                EmailAddress = Faker.Person.Email
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException) ,OnboardCustomerErrorConsts.OnboardCustomerNotExist);
        }

        [Test]
        public async Task Should_failure_while_updating_onboard_customer_when_onboard_customer_is_in_completed_state()
        {
            string userId = Guid.NewGuid().ToString();

            await CreateCompletedOnboardCustomer(userId);

            var command = new UpdateOnboardCustomerEmailCommand
            {
                UserId = userId,
                EmailAddress = Faker.Person.Email
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException), OnboardCustomerErrorConsts.OnboardCustomerCompleted);
        }
    }
}
