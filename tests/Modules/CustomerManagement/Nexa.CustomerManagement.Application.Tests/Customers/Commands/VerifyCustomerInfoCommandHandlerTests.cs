using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.CustomerManagement.Application.Customers.Commands.VerifyCustomerInfo;
using Nexa.Application.Tests.Extensions;
using FluentAssertions;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Domain.Exceptions;
namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    [TestFixture]
    public class VerifyCustomerInfoCommandHandlerTests : CustomerTestFixture
    {
        public ICustomerManagementRepository<Customer> CustomerRepository { get; }

        public VerifyCustomerInfoCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [TestCase(VerificationState.Pending)]
        [TestCase(VerificationState.Rejected)]
        public async Task Should_verify_customer_info(VerificationState state)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId,state);

            var command = new VerifyCustomerInfoCommand { };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository
                .AsQuerable()
                .SingleAsync(x => x.Id == fakeCustomer.Id);

            customer.InfoVerificationState.Should().Be(VerificationState.Processing);

            result.Value!.AssertCustomerDto(customer);
        }

        [Test]
        public async Task Should_failure_while_verifing_customer_info_when_user_is_not_authorized()
        {
            var command = new VerifyCustomerInfoCommand { };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }

        [Test]
        public async Task Should_failure_while_verifiing_customer_info_when_user_customer_is_not_exist()
        {
            AuthenticationService.Login();

            var command = new VerifyCustomerInfoCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [Test]
        public async Task Should_failure_while_verifiing_customer_info_when_customer_info_is_incomplete()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            await CreateCustomerWithoutInfo(userId);

            var command = new VerifyCustomerInfoCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [TestCase(VerificationState.Processing)]
        [TestCase(VerificationState.Verified)]
        public async Task Should_failure_while_verifiing_customer_info_verification_state_is_invalid(VerificationState state)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            await CreateCustomerAsync(userId,state);

            var command = new VerifyCustomerInfoCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }
    }
}
