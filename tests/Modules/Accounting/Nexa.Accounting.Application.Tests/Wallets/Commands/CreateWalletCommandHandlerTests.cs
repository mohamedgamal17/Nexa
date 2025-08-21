using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Tests.Fakers;
using Nexa.Accounting.Application.Wallets.Commands.CreateWallet;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Wallets;
using Nexa.Accounting.Shared.Enums;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
namespace Nexa.Accounting.Application.Tests.Wallets.Commands
{
    [TestFixture]
    public class CreateWalletCommandHandlerTests : AccountingTestFixture
    {
        protected IAccountingRepository<Wallet> WalletRepository { get; }
        public CreateWalletCommandHandlerTests()
        {
            WalletRepository = ServiceProvider.GetRequiredService<IAccountingRepository<Wallet>>();
        }

        [Test]
        public async Task Should_create_user_wallet()
        {

            var command = new CreateWalletCommand()
            {
                UserId = Guid.NewGuid().ToString(),
                CustomerId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var wallet = await WalletRepository.FindByIdAsync(result.Value!.Id);

            wallet.Should().NotBeNull();

            wallet!.State.Should().Be(WalletState.Frozen);
        }

    }
}
