using Nexa.Application.Tests.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.AcceptCustomer;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using FluentAssertions;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    [TestFixture]
    public class AcceptCustomerCommandHandlerTests : CustomerTestFixture
    {
        protected ICustomerManagementRepository<Customer> CustomerRepository { get; }

        public AcceptCustomerCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();

        }
        [Test]
        public async Task Should_accept_customer()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateReviewedCustomer(userId);

            var command = new AcceptCustomerCommand
            {
                FintechCustomerId = fakeCustomer.FintechCustomerId!
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await  CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            customer.State.Should().Be(VerificationState.Verified);
        }

    }
}
