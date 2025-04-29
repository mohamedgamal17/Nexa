using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.Accounting.Domain.Enums;
using Nexa.Accounting.Domain.Transactions;
using Nexa.Accounting.Domain.Wallets;
namespace Nexa.Accounting.Infrastructure.EntityFramework.Configurations
{
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(TransactionTableConsts.IdLength);

            builder.Property(x => x.Number).HasMaxLength(TransactionTableConsts.NumberLength);

            builder.Property(x => x.WalletId).HasMaxLength(TransactionTableConsts.WalletIdLength);

            builder.HasDiscriminator<TransactionType>(TransactionTableConsts.Type)
                .HasValue<InternalTransaction>(TransactionType.Internal)
                .HasValue<ExternalTransaction>(TransactionType.External);

            builder.HasOne<Wallet>().WithMany().HasForeignKey(x => x.WalletId).OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.Number).IsUnique();   
        }
    }

    public class InternalTransactionEntityTypeConfiguration : IEntityTypeConfiguration<InternalTransaction>
    {
        public void Configure(EntityTypeBuilder<InternalTransaction> builder)
        {
            builder.Property(x => x.ReciverId).HasMaxLength(TransactionTableConsts.ReciverIdLength);

            builder.HasOne<Wallet>().WithMany().HasForeignKey(x => x.ReciverId).OnDelete(DeleteBehavior.SetNull);
        }
    }

    public class ExternalTransactionEntityTypeConfiguration : IEntityTypeConfiguration<ExternalTransaction>
    {
        public void Configure(EntityTypeBuilder<ExternalTransaction> builder)
        {
            builder.Property(x => x.PaymentId).HasMaxLength(TransactionTableConsts.PaymentIdLength);
        }
    }
}
