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

            builder.Property(x => x.FirstName).HasMaxLength(CustomerTableConstants.FirstNameLength);

            builder.Property(x => x.LastName).HasMaxLength(CustomerTableConstants.LastNameLength);

            builder.Property(x => x.PhoneNumber).HasMaxLength(CustomerTableConstants.PhoneNumberLength);

            builder.Property(x => x.EmailAddress).HasMaxLength(CustomerTableConstants.EmailAddressLength); 

            builder.HasIndex(x => x.UserId);
        }
    }
}
