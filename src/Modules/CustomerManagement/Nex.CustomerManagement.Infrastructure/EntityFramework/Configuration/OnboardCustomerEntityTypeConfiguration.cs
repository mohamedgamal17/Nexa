using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.OnboardCustomers;

namespace Nexa.CustomerManagement.Infrastructure.EntityFramework.Configuration
{
    public class OnboardCustomerEntityTypeConfiguration : IEntityTypeConfiguration<OnboardCustomer>
    {
        public void Configure(EntityTypeBuilder<OnboardCustomer> builder)
        {
            builder.ToTable(OnboardCustomerTableConsts.TableName);

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).HasMaxLength(OnboardCustomerTableConsts.IdLength);

            builder.Property(x => x.UserId).HasMaxLength(OnboardCustomerTableConsts.UserIdLength);

            builder.Property(x => x.EmailAddress)
                   .IsRequired(false)
                   .HasMaxLength(OnboardCustomerTableConsts.EmailAddressLength);

            builder.Property(x => x.PhoneNumber)
                   .IsRequired(false)
                   .HasMaxLength(OnboardCustomerTableConsts.PhoneNumberLength);

            builder.OwnsOne(x => x.Info, navigationBuilder =>
            {
                navigationBuilder.ToTable(OnboardCustomerTableConsts.CustomerInfoTableName);

                navigationBuilder.Property(x => x.FirstName).HasMaxLength(CustomerInfoTableConsts.FirstNameLength);
                navigationBuilder.Property(x => x.LastName).HasMaxLength(CustomerInfoTableConsts.LastNameLength);
        
            });


            builder.OwnsOne(x => x.Address, addressNavigationBuilder =>
            {
                addressNavigationBuilder.ToTable(OnboardCustomerTableConsts.AddressTableName);
                addressNavigationBuilder.Property(x => x.Country).HasMaxLength(AddressTableConstants.CountryLength);
                addressNavigationBuilder.Property(x => x.City).HasMaxLength(AddressTableConstants.CityLength);
                addressNavigationBuilder.Property(x => x.State).HasMaxLength(AddressTableConstants.StateLength);
                addressNavigationBuilder.Property(x => x.StreetLine).HasMaxLength(AddressTableConstants.StreetLineLength);
                addressNavigationBuilder.Property(x => x.PostalCode).IsRequired(false).HasMaxLength(AddressTableConstants.PostalCodeLength);
                addressNavigationBuilder.Property(x => x.ZipCode).IsRequired(false).HasMaxLength(AddressTableConstants.ZipCodeLength);
            });

            builder.HasIndex(x => x.UserId).IsUnique();
        }
    }
}
