using FluentAssertions;
using Nexa.Accounting.Shared.Consts;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.Transactions.Application.Tests.Assertions;
using Nexa.Transactions.Application.Transfers.Commands.CreateNetworkTransfer;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Enums;
namespace Nexa.Transactions.Application.Tests.Transfers.Commands
{
    [TestFixture]
    public class CreateNetworkTransferCommandHandlerTests : TransferTestFixture
    {

        [Test]
        [TestCase(100, 50)]
        [TestCase(100, 100)]
        public async Task Should_create_network_transfer(decimal balance , decimal transferAmount)
        {
            AuthenticationService.Login();
            string userId = AuthenticationService.GetCurrentUser()!.Id;
            var senderWallet = await CreateWalletAsync(userId, balance);
            var reciverWallet = await CreateWalletAsync();

            var command = new CreateNetworkTransferCommand
            {
                SenderId = senderWallet.Id,
                ReciverId = reciverWallet.Id,
                Amount = transferAmount
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var transfer = (await TransferRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id)) as NetworkTransfer;

            transfer.Should().NotBeNull();

            transfer!.Status.Should().Be(TransferStatus.Pending);
            transfer.Amount.Should().Be(transferAmount);
            transfer.WalletId.Should().Be(senderWallet.Id);
            transfer.ReciverId.Should().Be(reciverWallet.Id);

            result.Value.Should().NotBeNull();
            result.Value!.AssertNetworkTransferDto(transfer);
        }


        [Test]
        public async Task Should_failure_while_creating_newtork_transfer_when_user_is_not_authorized()
        {
            var command = new CreateNetworkTransferCommand
            {
                SenderId = Guid.NewGuid().ToString(),
                ReciverId = Guid.NewGuid().ToString(),
                Amount = 50
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(NexaUnauthorizedAccessException), GlobalErrorConsts.UnauthorizedAccess);
        }

        [Test]
        public async Task Should_failure_while_creating_network_transfer_when_user_does_not_own_sender_wallet()
        {
            AuthenticationService.Login();

            var senderWallet = await CreateWalletAsync(balance: 100);

            var reciverWallet = await CreateWalletAsync();


            var command = new CreateNetworkTransferCommand
            {
                SenderId = senderWallet.Id,
                ReciverId = reciverWallet.Id,
                Amount = 50
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(ForbiddenAccessException), WalletErrorConsts.WalletNotOwned);
        }


        [Test]
        public async Task Should_failure_while_creating_newtork_transfer_when_wallet_balance_is_not_enough()
        {
            AuthenticationService.Login();
            string userId = AuthenticationService.GetCurrentUser()!.Id;
            var senderWallet = await CreateWalletAsync(userId, 100);
            var reciverWallet = await CreateWalletAsync();

            var command = new CreateNetworkTransferCommand
            {
                SenderId = senderWallet.Id,
                ReciverId = reciverWallet.Id,
                Amount = 150
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException), WalletErrorConsts.InsufficentBalance);
        }
    }
}
