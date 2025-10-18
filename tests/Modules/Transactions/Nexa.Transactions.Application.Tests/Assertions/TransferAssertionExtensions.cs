using FluentAssertions;
using Nexa.Transactions.Application.Transfers.Commands.CreateBankTransfer;
using Nexa.Transactions.Application.Transfers.Dtos;
using Nexa.Transactions.Domain.Transfers;

namespace Nexa.Transactions.Application.Tests.Assertions
{
    public static class TransferAssertionExtensions
    {
        public static void AssertTransferDto(this TransferDto dto , Transfer transfer)
        {
            dto.Id.Should().Be(transfer.Id);
            dto.UserId.Should().Be(transfer.UserId);
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

        public static void AssertBankTransfer(this BankTransfer bankTransfer , CreateBankTransferCommand command , string userId)
        {
            bankTransfer.UserId.Should().Be(userId);
            bankTransfer.WalletId.Should().Be(command.WalletId);
            bankTransfer.FundingResourceId.Should().Be(command.FundingResourceId);
            bankTransfer.Amount.Should().Be(command.Amount);
            bankTransfer.Direction.Should().Be(command.Direction);

        }

        public static void AssertBankTransferDto(this TransferDto dto , BankTransfer bankTransfer)
        {
            dto.AssertTransferDto(bankTransfer);
            dto.FundingResourceId.Should().Be(bankTransfer.FundingResourceId);
            dto.Direction.Should().Be(bankTransfer.Direction);
        }
    }
}
