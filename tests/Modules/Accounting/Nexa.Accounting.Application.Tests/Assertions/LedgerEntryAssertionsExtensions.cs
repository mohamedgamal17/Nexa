using FluentAssertions;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Wallets;

namespace Nexa.Accounting.Application.Tests.Assertions
{
    public static class LedgerEntryAssertionsExtensions
    {

        public static void AssertLedgerEntry(this LedgerEntry ledgerEntry, string walletId , string transactionId , decimal amount , TransactionType transactionType , TransactionDirection direction)
        {
            ledgerEntry.WalletId.Should().Be(walletId);
            ledgerEntry.TransactionId.Should().Be(transactionId);
            ledgerEntry.Amount.Should().Be(amount);
            ledgerEntry.Type.Should().Be(transactionType);
            ledgerEntry.Direction.Should().Be(direction);
        }
    }
}
