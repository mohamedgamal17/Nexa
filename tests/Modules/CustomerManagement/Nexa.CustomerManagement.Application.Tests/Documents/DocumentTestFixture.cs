using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Enums;

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
                    FirstName = Faker.Person.FirstName,
                    LastName = Faker.Person.LastName,
                    Gender = Faker.PickRandom<Gender>(),
                    EmailAddress = Faker.Person.Email,
                    PhoneNumber = "13322767084",
                    BirthDate = DateTime.Now.AddYears(-25),               
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

        protected async Task<DocumentAttachment> CreateDocumentAttachmentAsync(string documentId,  string userId, DocumentSide documentSide)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var documentRepository = sp.GetRequiredService<ICustomerManagementRepository<Document>>();

                var document = await documentRepository.SingleAsync(x => x.Id == documentId);

                var attachment = new DocumentAttachment(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 6555, "img/png", documentSide);

                document.AddAttachment(attachment);

                await documentRepository.UpdateAsync(document);

                return attachment;
            });
        }
    }
}
