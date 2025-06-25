using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.Accounting.Domain.Wallets;
namespace Nexa.Accounting.Infrastructure.EntityFramework.Configurations
{
    public class WalletEntityTypeConfiguration : IEntityTypeConfiguration<Wallet>
    {
        public void Configure(EntityTypeBuilder<Wallet> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(WalletTableConsts.IdLength);

            builder.Property(x => x.Number).HasMaxLength(WalletTableConsts.NumberLength);

            builder.Property(x => x.UserId).HasMaxLength(WalletTableConsts.UserIdLength);

            builder.HasIndex(x => x.Number).IsUnique();

            builder.HasIndex(x => x.UserId);

        }
    }

   
}
