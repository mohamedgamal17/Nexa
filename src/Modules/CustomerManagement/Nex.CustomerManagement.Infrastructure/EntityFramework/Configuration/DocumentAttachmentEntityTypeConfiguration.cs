//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Nexa.CustomerManagement.Domain.Documents;
//namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Configuration
//{
//    public class DocumentAttachmentEntityTypeConfiguration : IEntityTypeConfiguration<DocumentAttachment>
//    {
//        public void Configure(EntityTypeBuilder<DocumentAttachment> builder)
//        {
//            builder.Property(x => x.Id);

//            builder.Property(x => x.Id).HasMaxLength(DocumentAttachmentTableConsts.IdLength);

//            builder.Property(x => x.KYCExternalId).IsRequired(false).HasMaxLength(DocumentAttachmentTableConsts.KYCExternalIdLength);

//            builder.Property(x => x.FileName).HasMaxLength(DocumentAttachmentTableConsts.FileNameLength);

//            builder.Property(x => x.ContentType).HasMaxLength(DocumentAttachmentTableConsts.ContentTypeLength);

//            builder.HasIndex(x => x.KYCExternalId);

//        }
//    }
//}
