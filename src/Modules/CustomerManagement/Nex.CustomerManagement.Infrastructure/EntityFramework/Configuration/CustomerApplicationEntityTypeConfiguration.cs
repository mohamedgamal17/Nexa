using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.CustomerManagement.Domain.CustomerApplications;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Configuration
{
    public class CustomerApplicationEntityTypeConfiguration : IEntityTypeConfiguration<CustomerApplication>
    {
        public void Configure(EntityTypeBuilder<CustomerApplication> builder)
        {
            builder.ToTable(CustomerApplicationTableConsts.TableName);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(CustomerApplicationTableConsts.IdLength);

            builder.Property(x => x.CustomerId).HasMaxLength(CustomerApplicationTableConsts.CustomerIdLength);

            builder.Property(x => x.KycExternalId).HasMaxLength(CustomerApplicationTableConsts.KYCExternalIdLength);

            builder.Property(x => x.KycCheckId)
                .HasMaxLength(CustomerApplicationTableConsts.KYCCheckIdLength)
                .IsRequired(false);


            builder.Property(x => x.CustomerApplicationExternalId)
                .HasMaxLength(CustomerApplicationTableConsts.CustomerIdExternalIdLength)
                .IsRequired(false);


            builder.Property(x => x.FirstName).HasMaxLength(CustomerApplicationTableConsts.FirstNameLength);

            builder.Property(x => x.LastName).HasMaxLength(CustomerApplicationTableConsts.LastNameLength);

            builder.Property(x => x.MiddleName)
                .HasMaxLength(CustomerApplicationTableConsts.MiddleNameLength)
                .IsRequired(false);

            builder.Property(x => x.Nationality).HasMaxLength(CustomerApplicationTableConsts.NationalityLength);

            builder.Property(x => x.PhoneNumber).HasMaxLength(CustomerApplicationTableConsts.PhoneNumberLength);

            builder.Property(x => x.EmailAddress).HasMaxLength(CustomerApplicationTableConsts.EmailAddressLength);


            builder.OwnsOne(x => x.Address, navigationBuilder => 
            {
                navigationBuilder.Property(x => x.Country).HasMaxLength(AddressTableConstants.CountryLength);

                navigationBuilder.Property(x => x.City).HasMaxLength(AddressTableConstants.CityLength);

                navigationBuilder.Property(x => x.State).HasMaxLength(AddressTableConstants.StateLength);

                navigationBuilder.Property(x => x.StreetLine1).HasMaxLength(AddressTableConstants.StreetLineLength);

                navigationBuilder.Property(x => x.StreetLine2).IsRequired(false).HasMaxLength(AddressTableConstants.StreetLineLength);

                navigationBuilder.Property(x => x.PostalCode).IsRequired(false).HasMaxLength(AddressTableConstants.PostalCodeLength);

                navigationBuilder.Property(x => x.ZipCode).IsRequired(false).HasMaxLength(AddressTableConstants.ZipCodeLength);
            });

            builder.HasOne<Customer>().WithMany().HasForeignKey(x => x.CustomerId);

            builder.HasMany(x => x.Documents).WithOne().HasForeignKey(x => x.CustomerApplicationId);

            builder.HasIndex(x => x.KycCheckId); 

            builder.HasIndex(x => x.KycExternalId);

            builder.HasIndex(x => x.CustomerApplicationExternalId);
        }
    }
}
