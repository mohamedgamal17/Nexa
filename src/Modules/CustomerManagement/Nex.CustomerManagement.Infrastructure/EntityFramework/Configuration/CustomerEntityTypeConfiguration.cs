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

            builder.Property(x => x.KYCExternalId).IsRequired(false).HasMaxLength(CustomerTableConstants.KYCExternalIdLength);

            builder.Property(x => x.FirstName).HasMaxLength(CustomerTableConstants.FirstNameLength);

            builder.Property(x => x.MiddleName).HasMaxLength(CustomerTableConstants.FirstNameLength);

            builder.Property(x => x.LastName).HasMaxLength(CustomerTableConstants.LastNameLength);

            builder.Property(x => x.Nationality).HasMaxLength(CustomerTableConstants.NationalityLength);

            builder.Property(x => x.PhoneNumber).HasMaxLength(CustomerTableConstants.PhoneNumberLength);

            builder.Property(x => x.EmailAddress).HasMaxLength(CustomerTableConstants.EmailAddressLength);

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

            builder.HasIndex(x => x.UserId);

            builder.HasIndex(x => x.KYCExternalId);

        }
    }
}
