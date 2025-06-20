using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;
namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Configuration
{
    public class DocumentEntityTypeConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(DocumentTableConsts.IdLength);

            builder.Property(x => x.UserId).HasMaxLength(DocumentTableConsts.UserIdLength);

            builder.Property(x => x.CustomerId).HasMaxLength(DocumentTableConsts.CustomerIdLength);

            builder.Property(x => x.KYCExternalId).IsRequired(false).HasMaxLength(DocumentTableConsts.KYCExternalIdLength);

            builder.Property(x => x.IssuingCountry).HasMaxLength(DocumentTableConsts.IssuingCountryLength);

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.KYCExternalId);

            builder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId);

            builder.HasMany(x => x.Attachments).WithOne();

            builder.Navigation(x => x.Attachments).AutoInclude();
        }
    }
}
