using FluentAssertions;
using Nexa.CustomerManagement.Shared.Events;

namespace Nexa.Accounting.Application.Tests.Wallets.Consumers
{
    [TestFixture]
    public class When_customer_created_integration_event_consumed : WalletTestFixture
    {
        [Test]
        public async Task Should_create_user_wallet()
        {
            AuthenticationService.Login();

            var @event = new CustomerCreatedIntegrationEvent
            {
                UserId = AuthenticationService.GetCurrentUser()!.Id,
                CustomerId = Guid.NewGuid().ToString(),
                EmailAddress = Faker.Person.Email,
                PhoneNumber = Faker.Person.Phone
            };

            await TestHarness.Bus.Publish(@event);

            Assert.That(await TestHarness.Consumed.Any<CustomerCreatedIntegrationEvent>());

            var fakeWallet = WalletRepository.SingleOrDefaultAsync(x => x.UserId == @event.UserId);

            fakeWallet.Should().NotBeNull();
        }
    }
}
