using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.Accounting.Domain.FundingResources;

namespace Nexa.Accounting.Infrastructure.EntityFramework.Configurations
{
    public class BankAccountEntityTypeConfiguration : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> builder)
        {
            builder.ToTable(BankAccountTableConsts.TableName);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(BankAccountTableConsts.IdLength);

            builder.Property(x => x.UserId).HasMaxLength(BankAccountTableConsts.UserIdLength);

            builder.Property(x => x.CustomerId).HasMaxLength(BankAccountTableConsts.CustomerIdLength);

            builder.Property(x => x.ProviderBankAccountId).HasMaxLength(BankAccountTableConsts.ProviderBankAccountIdLength);

            builder.Property(x => x.HolderName)
                .HasMaxLength(BankAccountTableConsts.HolderNameLength)
                .IsRequired(false);

            builder.Property(x => x.BankName)
                .HasMaxLength(BankAccountTableConsts.BankNameLength)
                .IsRequired(false);

            builder.Property(x => x.Country).HasMaxLength(BankAccountTableConsts.CountryLength);

            builder.Property(x => x.Currency).HasMaxLength(BankAccountTableConsts.CurrencyLength);

            builder.Property(x => x.RoutingNumber).HasMaxLength(BankAccountTableConsts.RoutingNumberLength);

            builder.Property(x => x.AccountNumberLast4).HasMaxLength(BankAccountTableConsts.AccountNumberLast4Length);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.CustomerId);

            builder.HasIndex(x => x.ProviderBankAccountId);

        }
    }
}
