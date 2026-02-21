using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Domain.Reviews;
namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Configuration
{

    public class CustomerEntityTypeConfiguration : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable(CustomerTableConstants.TableName);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(CustomerTableConstants.IdLength);

            builder.Property(x => x.UserId).HasMaxLength(CustomerTableConstants.UserIdLength);

            builder.Property(x => x.KycCustomerId).HasMaxLength(CustomerTableConstants.KycCustomerId);

            builder.Property(x => x.FintechCustomerId).HasMaxLength(CustomerTableConstants.FintechCustomerId);

            builder.Property(x => x.PhoneNumber).HasMaxLength(CustomerTableConstants.PhoneNumberLength);

            builder.Property(x => x.EmailAddress).HasMaxLength(CustomerTableConstants.EmailAddressLength);

            builder.OwnsOne(x => x.Info, navigationBuilder =>
            {
                navigationBuilder.ToTable(CustomerInfoTableConsts.TableName);
                navigationBuilder.Property(x => x.FirstName).HasMaxLength(CustomerInfoTableConsts.FirstNameLength);
                navigationBuilder.Property(x => x.LastName).HasMaxLength(CustomerInfoTableConsts.LastNameLength);
               
            });

            builder.OwnsOne(x => x.Address, addressNavigationBuilder =>
            {
                addressNavigationBuilder.ToTable(AddressTableConstants.TableName);
                addressNavigationBuilder.Property(x => x.Country).HasMaxLength(AddressTableConstants.CountryLength);
                addressNavigationBuilder.Property(x => x.City).HasMaxLength(AddressTableConstants.CityLength);
                addressNavigationBuilder.Property(x => x.State).HasMaxLength(AddressTableConstants.StateLength);
                addressNavigationBuilder.Property(x => x.StreetLine).HasMaxLength(AddressTableConstants.StreetLineLength);
                addressNavigationBuilder.Property(x => x.PostalCode).IsRequired(false).HasMaxLength(AddressTableConstants.PostalCodeLength);
                addressNavigationBuilder.Property(x => x.ZipCode).IsRequired(false).HasMaxLength(AddressTableConstants.ZipCodeLength);
            });


            builder.OwnsOne(x => x.Document, navigationBuilder =>
            {
                navigationBuilder.ToTable(DocumentTableConsts.TableName);
                navigationBuilder.HasKey(x => x.Id);
                navigationBuilder.Property(x => x.Id).HasMaxLength(DocumentTableConsts.IdLength);
                navigationBuilder.Property(x => x.KycDocumentId).HasMaxLength(DocumentTableConsts.KycDocumentId);
                navigationBuilder.Property(x => x.IssuingCountry).IsRequired(false).HasMaxLength(DocumentTableConsts.IssuingCountryLength);
                navigationBuilder.Property(x => x.KycReviewId).IsRequired(false).HasMaxLength(DocumentTableConsts.KycReviewIdLength);
                navigationBuilder.HasOne<KycReview>().WithMany().HasForeignKey(x => x.KycReviewId);
                navigationBuilder.HasIndex(x => x.KycDocumentId);
                navigationBuilder.OwnsMany(x => x.Attachments, attachmentNavigationbuilder =>
                {
                    attachmentNavigationbuilder.ToTable(DocumentAttachmentTableConsts.TableName);

                    attachmentNavigationbuilder.Property(x => x.Id);

                    attachmentNavigationbuilder.Property(x => x.Id).HasMaxLength(DocumentAttachmentTableConsts.IdLength);

                    attachmentNavigationbuilder.Property(x => x.KycAttachmentId).IsRequired(false).HasMaxLength(DocumentAttachmentTableConsts.KycAttachmentIdLength);

                    attachmentNavigationbuilder.Property(x => x.FileName).HasMaxLength(DocumentAttachmentTableConsts.FileNameLength);

                    attachmentNavigationbuilder.Property(x => x.ContentType).HasMaxLength(DocumentAttachmentTableConsts.ContentTypeLength);

                    attachmentNavigationbuilder.HasIndex(x => x.KycAttachmentId);

                });
                navigationBuilder.Navigation(x => x.Attachments).AutoInclude();

            });


            builder.HasIndex(x => x.UserId);
        }
    }
}
