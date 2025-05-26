using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nex.CustomerManagement.Infrastructure.EntityFramework.Configuration
{
    public class AddressEntityTypeConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(AddressTableConstants.IdLength);

            builder.Property(x => x.Country).HasMaxLength(AddressTableConstants.CountryLength);

            builder.Property(x => x.City).HasMaxLength(AddressTableConstants.CityLength);

            builder.Property(x => x.State).HasMaxLength(AddressTableConstants.StateLength);

            builder.Property(x => x.StreetLine1).HasMaxLength(AddressTableConstants.StreetLineLength);

            builder.Property(x => x.StreetLine2).IsRequired(false).HasMaxLength(AddressTableConstants.StreetLineLength);

            builder.Property(x => x.PostalCode).IsRequired(false).HasMaxLength(AddressTableConstants.PostalCodeLength);

            builder.Property(x => x.ZipCode).IsRequired(false).HasMaxLength(AddressTableConstants.ZipCodeLength);
        }
    }
}
