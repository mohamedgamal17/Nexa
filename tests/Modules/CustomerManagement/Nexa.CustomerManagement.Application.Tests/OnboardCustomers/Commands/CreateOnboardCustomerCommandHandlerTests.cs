using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.CreateOnboardCustomer;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.OnboardCustomers.Commands
{
    public class CreateOnboardCustomerCommandHandlerTests : OnboardCustomerTestFixture
    {
        public ICustomerManagementRepository<OnboardCustomer> onboardCustomerRepository { get; }


        public CreateOnboardCustomerCommandHandlerTests()
        {
            onboardCustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<OnboardCustomer>>();
        }


        [Test]
        public async Task Should_create_onboard_customer()
        {
            string userId = Guid.NewGuid().ToString();

            var command = new CreateOnboardCustomerCommand
            {
                UserId = userId
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var onboardCustomer = await onboardCustomerRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id);

            onboardCustomer.Should().NotBeNull();

            onboardCustomer!.Status.Should().Be(OnboardCustomerStatus.Started);
        }

        [Test]
        public async Task Should_failure_while_creating_onboard_customer_when_onboard_customer_is_already_created_for_the_same_user()
        {
            string userId = Guid.NewGuid().ToString();

            await CreateInitialOnboardCustomerAsync(userId);

            var command = new CreateOnboardCustomerCommand
            {
                UserId = userId
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(ConflictException));
        }

    }
}
