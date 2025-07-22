using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.CustomerManagement.Domain.Documents;
namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Configuration
{
    public class DocumentEntityTypeConfiguration : IEntityTypeConfiguration<Document>
    {
        public void Configure(EntityTypeBuilder<Document> builder)
        {
            builder.ToTable(DocumentTableConsts.TableName);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(DocumentTableConsts.IdLength);

            builder.Property(x => x.CustomerId).HasMaxLength(DocumentTableConsts.CustomerIdLength);

            builder.Property(x => x.KYCExternalId).IsRequired(false).HasMaxLength(DocumentTableConsts.KYCExternalIdLength);

            builder.HasIndex(x => x.KYCExternalId);

            builder.HasMany(x => x.Attachments).WithOne();

            builder.Navigation(x => x.Attachments).AutoInclude();
        }
    }
}
