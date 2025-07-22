using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.CustomerManagement.Domain.Customers;
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

            builder.Property(x => x.FintechCustomerid).HasMaxLength(CustomerTableConstants.FintechCustomerId);

            builder.Property(x => x.PhoneNumber).HasMaxLength(CustomerTableConstants.PhoneNumberLength);

            builder.Property(x => x.EmailAddress).HasMaxLength(CustomerTableConstants.EmailAddressLength);

            builder.OwnsOne(x => x.Info, navigationBuilder =>
            {
                navigationBuilder.ToTable(CustomerInfoTableConsts.TableName);
                navigationBuilder.Property(x => x.FirstName).HasMaxLength(CustomerInfoTableConsts.FirstNameLength);
                navigationBuilder.Property(x => x.LastName).HasMaxLength(CustomerInfoTableConsts.LastNameLength);
                navigationBuilder.Property(x => x.IdNumber).HasMaxLength(CustomerInfoTableConsts.IdNumberLength);
                navigationBuilder.Property(x => x.Nationality).HasMaxLength(CustomerInfoTableConsts.NationalityLength);

                navigationBuilder.OwnsOne(x => x.Address, addressNavigationBuilder =>
                {
                    addressNavigationBuilder.Property(x => x.Country).HasMaxLength(AddressTableConstants.CountryLength);
                    addressNavigationBuilder.Property(x => x.City).HasMaxLength(AddressTableConstants.CityLength);
                    addressNavigationBuilder.Property(x => x.State).HasMaxLength(AddressTableConstants.StateLength);
                    addressNavigationBuilder.Property(x => x.StreetLine).HasMaxLength(AddressTableConstants.StreetLineLength);
                    addressNavigationBuilder.Property(x => x.PostalCode).IsRequired(false).HasMaxLength(AddressTableConstants.PostalCodeLength);
                    addressNavigationBuilder.Property(x => x.ZipCode).IsRequired(false).HasMaxLength(AddressTableConstants.ZipCodeLength);
                });
            });

            builder.HasMany(x => x.Documents).WithOne().HasForeignKey(x => x.CustomerId);

            builder.HasIndex(x => x.UserId);
        }
    }
}
