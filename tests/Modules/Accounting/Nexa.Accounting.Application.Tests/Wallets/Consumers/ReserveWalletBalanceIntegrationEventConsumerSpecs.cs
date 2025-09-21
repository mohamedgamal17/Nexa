using FluentAssertions;
using Nexa.Accounting.Shared.Events;

namespace Nexa.Accounting.Application.Tests.Wallets.Consumers
{
    [TestFixture]
    public class When_reserve_wallet_balance_event_consumed : WalletTestFixture
    {
        [Test]
        public async Task Should_reserve_wallet_balance_and_publish_wallet_balance_reserved_event()
        {
            var fakeWallet = await CreateWalletAsync(balance : 1000);

            var message = new ReserveWalletBalanceIntegrationEvent()
            {
                TransferId = Guid.NewGuid().ToString(),
                WalletId = fakeWallet.Id,
                Amount = 200
            };

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<ReserveWalletBalanceIntegrationEvent>());
            Assert.That(await TestHarness.Published.Any<TransferVerifiedIntegrationEvent>());

            var wallet = await WalletRepository.SingleAsync(x => x.Id == fakeWallet.Id);

            wallet.ReservedBalance.Should().Be(200);
            wallet.Balance.Should().Be(800);
        }

        [Test]
        public async Task Should_publish_wallet_balance_reservation_faild_when_wallet_balance_is_insufficient()
        {
            var fakeWallet = await CreateWalletAsync(balance: 200);

            var message = new ReserveWalletBalanceIntegrationEvent()
            {
                TransferId = Guid.NewGuid().ToString(),
                WalletId = fakeWallet.Id,
                Amount = 300
            };

            await TestHarness.Bus.Publish(message);

            Assert.That(await TestHarness.Consumed.Any<ReserveWalletBalanceIntegrationEvent>());
            Assert.That(await TestHarness.Published.Any<WalletBalanceReservationFailedIntegrationEvent>());
        }
    }
}
