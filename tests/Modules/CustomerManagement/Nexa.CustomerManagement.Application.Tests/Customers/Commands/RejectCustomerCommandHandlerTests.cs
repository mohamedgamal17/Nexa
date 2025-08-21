using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.RejectCustomer;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    [TestFixture]
    public class RejectCustomerCommandHandlerTests : CustomerTestFixture
    {
        protected ICustomerManagementRepository<Customer> CustomerRepository { get; }

        public RejectCustomerCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();

        }
        [Test]
        public async Task Should_reject_customer()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateReviewedCustomer(userId);

            var command = new RejectCustomerCommand
            {
                FintechCustomerId = fakeCustomer.FintechCustomerId!
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            customer.State.Should().Be(VerificationState.Rejected);

            customer.Info!.State.Should().Be(VerificationState.Rejected);

            customer.Document!.State.Should().Be(VerificationState.Rejected);
        }
    }
}
