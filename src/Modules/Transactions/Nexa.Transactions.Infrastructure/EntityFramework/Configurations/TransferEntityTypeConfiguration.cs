using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.Transactions.Domain.Transfers;
using Nexa.Transactions.Shared.Enums;

namespace Nexa.Transactions.Infrastructure.EntityFramework.Configurations
{
    public class TransferEntityTypeConfiguration : IEntityTypeConfiguration<Transfer>
    {
        public void Configure(EntityTypeBuilder<Transfer> builder)
        {
            builder.ToTable(TransferTableConsts.TableName);
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(TransferTableConsts.IdLength);

            builder.Property(x => x.UserId).HasMaxLength(TransferTableConsts.UserIdLength);

            builder.Property(x => x.Number).HasMaxLength(TransferTableConsts.NumberLength);

            builder.Property(x => x.WalletId).HasMaxLength(TransferTableConsts.WalletIdLength);

            builder.HasDiscriminator(x => x.Type)
                .HasValue<NetworkTransfer>(TransferType.Network)
                .HasValue<BankTransfer>(TransferType.Bank);


            builder.HasIndex(x => x.Number).IsUnique();

            builder.HasIndex(x => x.UserId);

            builder.Ignore(x => x.Events);

        }
    }

    public class NetworkTransferEntityTypeConfiguration : IEntityTypeConfiguration<NetworkTransfer>
    {
        public void Configure(EntityTypeBuilder<NetworkTransfer> builder)
        {
            builder.Property(x => x.ReciverId).HasMaxLength(TransferTableConsts.ReciverIdLength);

            builder.HasIndex(x=> x.ReciverId);
        }
    }

    public class BankTransferEntityTypeConfiguration : IEntityTypeConfiguration<BankTransfer>
    {
        public void Configure(EntityTypeBuilder<BankTransfer> builder)
        {
            builder.Property(x => x.FundingResourceId).HasMaxLength(TransferTableConsts.CounterPartyIdLength);

            builder.HasIndex(x => x.FundingResourceId);
        }
    }

    public class TransferViewEntityTypeConfiugration : IEntityTypeConfiguration<TransferView>
    {
        public void Configure(EntityTypeBuilder<TransferView> builder)
        {
            builder.HasNoKey();
            builder.ToView(TransferTableConsts.TableName);
        }
    }
}
