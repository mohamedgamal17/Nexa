using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Application.Tests.Documents
{
    public abstract class DocumentTestFixture : CustomerManagementTestFixture
    {
        protected Faker Faker { get; }

        protected DocumentTestFixture()
        {
            Faker = new Faker();
        }
        protected async Task<Customer> CreateCustomerAsync(string? userId = null)
        {

            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();


                var customer = new Customer
                {
                    UserId = userId ?? Guid.NewGuid().ToString(),
                    KYCExternalId = Guid.NewGuid().ToString(),
                    FirstName = Faker.Person.FirstName,
                    MiddleName = Faker.Person.LastName,
                    LastName = Faker.Person.LastName,
                    Gender = Faker.PickRandom<Gender>(),
                    EmailAddress = Faker.Person.Email,
                    PhoneNumber = "13322767084",
                    Nationality = "US",
                    BirthDate = DateTime.Now.AddYears(-25),
                    Address = new Address
                    {
                        Country = Faker.Address.CountryCode(),
                        State = Faker.Address.State(),
                        City = Faker.Address.City(),
                        StreetLine1 = Faker.Address.StreetAddress(),
                        StreetLine2 = Faker.Address.StreetAddress(),
                        PostalCode = Faker.Address.ZipCode(),
                        ZipCode = Faker.Address.ZipCode()
                    },

                };


                return await repository.InsertAsync(customer);
            });
        }
        protected async Task<Document> CreateDocumentAsync(string customerId, string userId, string issuingCountry, string kycExternalId, DocumentType type, bool isActive, DocumentStatus status, DateTime? verifiedAt, DateTime? rejctedAt)
        {
            return await WithScopeAsync(async sp =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Document>>();

                var document = new Document(customerId, userId, issuingCountry, kycExternalId, type,isActive, status, verifiedAt, rejctedAt);

                return await repository.InsertAsync(document);
            });
        }

        protected async Task<Document> CreatePendingDocumentAsync(string customerId, string userId, string issuingCountry, string kycExternalId, DocumentType type)
        {
            return await CreateDocumentAsync(customerId, userId, issuingCountry, kycExternalId, type, false, DocumentStatus.Pending, null, null);

        }
        protected async Task<Document> CreateActiveDocumentAsync(string customerId , string userId, string issuingCountry, string kycExternalId, DocumentType type)
        {

            return await CreateDocumentAsync(customerId, userId, issuingCountry, kycExternalId, type, true, DocumentStatus.Processing, null, null);
        }

        protected async Task<Document> CreateApprovedDocumentAsync(string customerId, string userId, string issuingCountry, string kycExternalId, DocumentType type)
        {
            return await CreateDocumentAsync(customerId, userId, issuingCountry, kycExternalId, type, false, DocumentStatus.Approved, DateTime.UtcNow, null);
        }


        protected async Task<Document> CreateRejectedDocumentAsync(string customerId, string userId, string issuingCountry, string kycExternalId, DocumentType type)
        {
            return await CreateDocumentAsync(customerId, userId, issuingCountry, kycExternalId, type, false, DocumentStatus.Rejected,null ,DateTime.UtcNow);
        }
    }
}
