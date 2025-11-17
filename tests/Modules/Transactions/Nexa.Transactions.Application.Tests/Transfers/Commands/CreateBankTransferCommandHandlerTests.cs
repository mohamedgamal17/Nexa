using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Accounting.Shared.Consts;
using Nexa.Accounting.Shared.Enums;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.Transactions.Application.Tests.Assertions;
using Nexa.Transactions.Application.Transfers.Commands.CreateBankTransfer;
using Nexa.Transactions.Domain;
using Nexa.Transactions.Domain.Transfers;
namespace Nexa.Transactions.Application.Tests.Transfers.Commands
{
    [TestFixture]
    public class CreateBankTransferCommandHandlerTests : TransferTestFixture
    {
        protected ITransactionRepository<BankTransfer> BankTransferRepository { get; }

        public CreateBankTransferCommandHandlerTests()
        {
            BankTransferRepository = ServiceProvider.GetRequiredService<ITransactionRepository<BankTransfer>>();
        }

        [TestCase(Shared.Enums.TransferDirection.Depit)]
        [TestCase(Shared.Enums.TransferDirection.Credit)]
        public async Task Should_create_bank_transfer(Shared.Enums.TransferDirection direction)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeWallet = await CreateWalletAsync(userId);

            var fakeFundingResource = await CreateFundingResourceAsync(userId);

            var command = new CreateBankTransferCommand
            {
                WalletId = fakeWallet.Id,
                FundingResourceId = fakeFundingResource.Id,
                Amount = 100,
                Direction = direction
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var bankTransfer = await BankTransferRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id);

            bankTransfer.Should().NotBeNull();

            bankTransfer!.AssertBankTransfer(command, userId);

            result.Value!.AssertTransferDto(bankTransfer!);
        }

        [Test]
        public async Task Should_failure_while_creating_bank_transfer_when_user_is_not_authorized()
        {
            var command = new CreateBankTransferCommand
            {
                WalletId = Guid.NewGuid().ToString(),
                FundingResourceId = Guid.NewGuid().ToString(),
                Amount = 100,
                Direction = Shared.Enums.TransferDirection.Credit
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(NexaUnauthorizedAccessException), GlobalErrorConsts.UnauthorizedAccess);
        }

      
        [Test]
        public async Task Should_failure_while_creating_bank_transfer_when_user_dose_not_own_current_wallet()
        {
            AuthenticationService.Login();

            var fakeWallet = await CreateWalletAsync();

            var command = new CreateBankTransferCommand
            {
                WalletId = fakeWallet.Id,
                FundingResourceId = Guid.NewGuid().ToString(),
                Amount = 100,
                Direction = Shared.Enums.TransferDirection.Credit
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(ForbiddenAccessException), WalletErrorConsts.WalletNotOwned);
        }


        [Test]
        public async Task Should_failure_while_creating_bank_transfer_when_user_dose_not_own_current_funding_resource()
        {
            AuthenticationService.Login();
            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeWallet = await CreateWalletAsync(userId);

            var fakeFundingResource = await CreateFundingResourceAsync();

            var command = new CreateBankTransferCommand
            {
                WalletId = fakeWallet.Id,
                FundingResourceId = fakeFundingResource.Id,
                Amount = 100,
                Direction = Shared.Enums.TransferDirection.Credit
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(ForbiddenAccessException), BankAccountErrorConsts.BankAccountNotOwned);
        }
    }
}
