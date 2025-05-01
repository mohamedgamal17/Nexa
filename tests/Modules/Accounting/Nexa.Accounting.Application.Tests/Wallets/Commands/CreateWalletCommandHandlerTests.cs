using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Application.Tests.Fakers;
using Nexa.Accounting.Application.Wallets.Commands.CreateWallet;
using Nexa.Accounting.Domain;
using Nexa.Accounting.Domain.Wallets;
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
            AuthenticationService.Login();

            var command = new CreateWalletCommand();

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var wallet = WalletRepository.FindByIdAsync(result.Value!.Id);

            wallet.Should().NotBeNull();
        }


        [Test]
        public async Task Should_failure_while_creating_user_wallet_when_user_is_not_authenticated()
        {
            var command = new CreateWalletCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }

        [Test]
        public async Task Should_failure_while_creating_user_wallet_when_user_is_already_has_wallet()
        {
            AuthenticationService.Login();

            var fakeWallet = new WalletFaker(AuthenticationService.GetCurrentUser()!.Id)
                .Generate(1)
                .First();

            await WalletRepository.InsertAsync(fakeWallet);

            var command = new CreateWalletCommand();

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }
    }
}
