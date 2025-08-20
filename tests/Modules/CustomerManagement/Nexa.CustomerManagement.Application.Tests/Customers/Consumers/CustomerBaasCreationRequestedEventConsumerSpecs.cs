using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.CustomerManagement.Shared.Events;
using Nexa.Integrations.Baas.Abstractions.Services;
namespace Nexa.CustomerManagement.Application.Tests.Customers.Consumers
{
    [TestFixture]
    public class  When_customer_baas_creation_requested_event_consumed  : CustomerTestFixture
    {
        protected ICustomerManagementRepository<Customer> CustomerRepository { get; }

        protected IBaasClientService FakeClientService { get; }

        public When_customer_baas_creation_requested_event_consumed()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
            FakeClientService = ServiceProvider.GetRequiredService<IBaasClientService>();
        }

        [Test]
        public async Task Should_create_customer_in_baas_api()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId); 

            await CreateDocumentWithAttachmentsAsync (fakeCustomer.Id, DocumentType.Passport);

            await AcceptCustomerInfo(fakeCustomer.Id);

            await AcceptCustomerDocument(fakeCustomer.Id);

            var @event = new CustomerBaasCreationRequestedEvent(fakeCustomer.Id);

            await TestHarness.Bus.Publish(@event);

            Assert.That(await TestHarness.Consumed.Any<CustomerBaasCreationRequestedEvent>());

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            var baasClient = await FakeClientService.GetClientAsync(customer.FintechCustomerId!);

            customer.FintechCustomerId.Should().NotBeNull();

            customer.FintechCustomerId.Should().Be(baasClient.Id);

            customer.State.Should().Be(VerificationState.Processing);
        }
    }
}
