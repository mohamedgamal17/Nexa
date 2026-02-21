using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.UpdateOnboardCustomerInfo;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.OnboardCustomers.Commands
{
    public class UpdateOnboardCustomerInfoCommandHandlerTests : OnboardCustomerTestFixture
    {
        public ICustomerManagementRepository<OnboardCustomer> OnboardCustomerRepository { get;  }

        public UpdateOnboardCustomerInfoCommandHandlerTests()
        {
            OnboardCustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<OnboardCustomer>>();
        }


        [Test]
        public async Task Should_update_onboard_customer_info()
        {
            string userId = Guid.NewGuid().ToString();

            var fakeOnboardCustomer = await CreateInitialOnboardCustomerAsync(userId);

            var command = new UpdateOnboardCustomerInfoCommand
            {
                UserId = userId,
                FirstName = Faker.Person.FirstName,
                LastName = Faker.Person.LastName,
                BirthDate = Faker.Person.DateOfBirth,
                Gender = Gender.Male
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var onboardCustomer = await OnboardCustomerRepository.SingleAsync(x => x.Id == fakeOnboardCustomer.Id);

            onboardCustomer.Info.Should().NotBeNull();
            onboardCustomer.Info!.FirstName.Should().Be(command.FirstName);
            onboardCustomer.Info!.LastName.Should().Be(command.LastName);
            onboardCustomer.Info!.Gender.Should().Be(command.Gender);
            onboardCustomer.Info!.BirthDate.Should().Be(command.BirthDate);
        }

        [Test]
        public async Task Should_failure_while_updating_onboard_customer_when_onboard_customer_is_not_exist()
        {
            string userId = Guid.NewGuid().ToString();

            var command = new UpdateOnboardCustomerInfoCommand
            {
                UserId = userId,
                FirstName = Faker.Person.FirstName,
                LastName = Faker.Person.LastName,
                BirthDate = Faker.Person.DateOfBirth,
                Gender = Gender.Male
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), OnboardCustomerErrorConsts.OnboardCustomerNotExist);
        }

        [Test]
        public async Task Should_failure_while_updating_onboard_customer_when_onboard_customer_is_in_complete_state()
        {
            string userId = Guid.NewGuid().ToString();

            await CreateCompletedOnboardCustomer(userId);

            var command = new UpdateOnboardCustomerInfoCommand
            {
                UserId = userId,
                FirstName = Faker.Person.FirstName,
                LastName = Faker.Person.LastName,
                BirthDate = Faker.Person.DateOfBirth,
                Gender = Gender.Male
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), OnboardCustomerErrorConsts.OnboardCustomerNotExist);
        }
    }
}
