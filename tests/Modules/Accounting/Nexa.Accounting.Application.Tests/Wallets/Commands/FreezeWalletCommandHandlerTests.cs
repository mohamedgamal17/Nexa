using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Wallets.Commands.FreezeWallet;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Enums;
using Nexa.Application.Tests.Extensions;

namespace Nexa.Accounting.Application.Tests.Wallets.Commands
{
    [TestFixture]
    public class FreezeWalletCommandHandlerTests : WalletTestFixture
    {
        public FreezeWalletCommandHandlerTests()
        {
        }

        [Test]
        public async Task Should_freeze_active_customer_wallet()
        {
            var fakeWallet = await CreateActiveWallet();

            var command = new FreezeWalletCommand
            {
                CustomerId = fakeWallet.CustomerId
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var wallet = await WalletRepository.SingleAsync(x => x.Id == fakeWallet.Id);

            wallet.State.Should().Be(WalletState.Frozen);
        }
    }
}
