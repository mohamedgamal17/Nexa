using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.Accounting.Domain.Wallets;
namespace Nexa.Accounting.Infrastructure.EntityFramework.Configurations
{
    public class LegerEntryEntityTypeConfiguration : IEntityTypeConfiguration<LedgerEntry>
    {
        public void Configure(EntityTypeBuilder<LedgerEntry> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(LedgerEntryTableConsts.IdLength);

            builder.Property(x => x.WalletId).HasMaxLength(LedgerEntryTableConsts.WalletIdLength);

            builder.Property(x => x.TransactionId).HasMaxLength(LedgerEntryTableConsts.TransactionIdLength);

            builder.HasOne<Wallet>().WithMany().HasForeignKey(x => x.WalletId);

        }
    }
}
