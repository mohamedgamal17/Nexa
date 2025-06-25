using FluentAssertions;
using Nexa.Transactions.Application.Transfers.Dtos;
using Nexa.Transactions.Domain.Transfers;

namespace Nexa.Transactions.Application.Tests.Assertions
{
    public static class TransferAssertionExtensions
    {
        public static void AssertTransferDto(this TransferDto dto , Transfer transfer)
        {
            dto.Id.Should().Be(transfer.Id);
            dto.Number.Should().Be(transfer.Number);
            dto.WalletId.Should().Be(transfer.WalletId);
            dto.Amount.Should().Be(transfer.Amount);
            dto.Type.Should().Be(transfer.Type);
            dto.Status.Should().Be(transfer.Status);
            dto.CompletedAt.Should().Be(transfer.CompletedAt);
        }
        public static void AssertNetworkTransferDto(this TransferDto dto , NetworkTransfer transfer)
        {
            dto.AssertTransferDto(transfer);
            dto.ReciverId.Should().Be(transfer.ReciverId);
        }
    }
}
