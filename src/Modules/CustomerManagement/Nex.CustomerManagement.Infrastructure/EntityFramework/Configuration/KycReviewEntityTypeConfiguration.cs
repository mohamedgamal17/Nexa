using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Reviews;

namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Configuration
{
    public class KycReviewEntityTypeConfiguration : IEntityTypeConfiguration<KycReview>
    {
        public void Configure(EntityTypeBuilder<KycReview> builder)
        {
            builder.ToTable(KycReviewTableConsts.TableName);

            builder.Property(x => x.Id).HasMaxLength(KycReviewTableConsts.IdLength);

            builder.Property(x => x.KycCheckId).HasMaxLength(KycReviewTableConsts.KycCheckId);

            builder.Property(x => x.KycLiveVideoId).IsRequired(false).HasMaxLength(KycReviewTableConsts.KycLiveVideoId);

            builder.HasIndex(x => x.KycCheckId);

            builder.HasIndex(x => x.KycLiveVideoId);

            builder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId);
        }
    }
}
