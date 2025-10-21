using FluentAssertions;
using MassTransit.Testing;
using Nexa.Accounting.Shared.Enums;
using Nexa.Transactions.Shared.Enums;
using Nexa.Transactions.Shared.Events;

namespace Nexa.Transactions.Application.Tests.Transfers.Consumers
{
    [TestFixture]
    public class When_process_bank_transfer_integration_event_cosumed : TransferTestFixture
    {
        [Test]
        public async Task Should_create_external_bank_transfer()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeWallet = await CreateWalletAsync(userId);

            var fakeFundingResource = await CreateFundingResourceAsync(userId);

            var fakeTransfer = await CreateProcessBankTransferAsync(userId, fakeWallet.Id, fakeFundingResource.Id,
                50, Shared.Enums.TransferDirection.Credit);

            await TestHarness.Start();

            var @event = new ProcessBankTransferIntegrationEvent(
                    userId,
                    fakeTransfer.Id,
                    fakeWallet.Id,
                    fakeFundingResource.Id,
                    fakeTransfer.Amount,
                    fakeTransfer.Direction,
                    BankTransferType.Ach
                );

            await TestHarness.Bus.Publish(@event);

            Assert.That(await TestHarness.Consumed.Any<ProcessBankTransferIntegrationEvent>());

            var transfer = await TransferRepository.SingleAsync(x => x.Id == fakeTransfer.Id);

            transfer.ExternalTransferId.Should().NotBeNull();

            await TestHarness.Stop();
        }

    }
}
