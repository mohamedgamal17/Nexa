using MediatR;
using Nexa.CustomerManagement.Domain.OnboardCustomers;
using Nexa.CustomerManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Application.OnboardCustomers.Commands.CompleteOnboardCustomer;
using Nexa.Application.Tests.Extensions;
using FluentAssertions;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Application.Tests.Assertions;

namespace Nexa.CustomerManagement.Application.Tests.OnboardCustomers.Commands
{
    public class CompleteOnboardCustomerCommandHandlerTests : OnboardCustomerTestFixture
    {
        public ICustomerManagementRepository<OnboardCustomer> OnboardCustomerRepository { get; }
        public ICustomerManagementRepository<Customer> CustomerRepository { get; set; }
        public CompleteOnboardCustomerCommandHandlerTests()
        {
            OnboardCustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<OnboardCustomer>>();

            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [Test]
        public async Task Should_complete_onboard_customer()
        {
            string userId = Guid.NewGuid().ToString();

            var fakeOnboardCustomer = await CreateFullDataCustomerAsync(userId);

            var command = new CompleteOnboardCustomerCommand
            {
                UserId = userId
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var onboardCustomer =await OnboardCustomerRepository.SingleAsync(x => x.Id == fakeOnboardCustomer.Id);

            onboardCustomer.Status.Should().Be(OnboardCustomerStatus.Completed);

            var customer = await CustomerRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id);

            customer.Should().NotBeNull();

            customer!.AssertCustomerToOnboard(onboardCustomer);
        }

        [Test]
        public async Task Should_failure_while_completing_onboard_customer_when_onboard_customer_is_not_exist()
        {
            string userId = Guid.NewGuid().ToString();

            var command = new CompleteOnboardCustomerCommand
            {
                UserId = userId
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), OnboardCustomerErrorConsts.OnboardCustomerNotExist);
        }

        [Test]
        public async Task Should_failure_while_completing_onbard_customer_when_onboard_customer_data_is_not_complete()
        {
            string userId = Guid.NewGuid().ToString();

            await CreateInitialOnboardCustomerAsync(userId);

            var command = new CompleteOnboardCustomerCommand
            {
                UserId = userId
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException), OnboardCustomerErrorConsts.OnboardCustomerIncomplete);

        }

        [Test]
        public async Task Should_failure_while_completing_onbard_customer_when_onboard_customer_status_is_completed()
        {
            string userId = Guid.NewGuid().ToString();

            await CreateCompletedOnboardCustomer(userId);

            var command = new CompleteOnboardCustomerCommand
            {
                UserId = userId
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException), OnboardCustomerErrorConsts.OnboardCustomerCompleted);

        }
    }
}
