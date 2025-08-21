using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Wallets.Commands.ActivateWallet;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Enums;
using Nexa.Application.Tests.Extensions;

namespace Nexa.Accounting.Application.Tests.Wallets.Commands
{
    [TestFixture]
    public class ActivateWalletCommandHandlerTests : WalletTestFixture
    {
        protected IWalletRepository WalletRepository { get; }

        public ActivateWalletCommandHandlerTests()
        {
            WalletRepository = ServiceProvider.GetRequiredService<IWalletRepository>();
        }


        [Test]
        public async Task Should_activate_customer_wallet()
        {
            var fakeWallet = await CreateWalletAsync();

            var command = new ActivateWalletCommand
            {
                FintechId = Guid.NewGuid().ToString(),
                CustomerId = fakeWallet.CustomerId
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var wallet = await WalletRepository.SingleAsync(x => x.Id == fakeWallet.Id);

            wallet.ProviderWalletId.Should().NotBeNull();

            wallet.State.Should().Be(WalletState.Active);
        }
    }
}
