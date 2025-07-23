using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.RejcetCustomerInfo;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    [TestFixture]
    public class RejectCustomerInfoCommandHandlerTests : CustomerTestFixture
    {
        protected ICustomerManagementRepository<Customer> CustomerRepository { get; }

        public RejectCustomerInfoCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [Test]
        public async Task Should_reject_customer_info()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId, VerificationState.InReview);

            var command = new RejectCustomerInfoCommand
            {
                KycCustomerId = fakeCustomer.KycCustomerId!
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.AsQuerable()
                .Include(x => x.Documents)
                .ThenInclude(c => c.Attachments)
                .SingleAsync(x => x.Id == fakeCustomer.Id);

            customer.InfoVerificationState.Should().Be(VerificationState.Verified);
        }
    }
}
