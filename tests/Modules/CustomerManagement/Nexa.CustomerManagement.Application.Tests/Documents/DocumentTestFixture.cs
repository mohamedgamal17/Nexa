using Bogus;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.CustomerApplications;
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

                var customer = new Customer(userId ?? Guid.NewGuid().ToString(), Faker.Person.Phone, Faker.Person.Email);
               
                return await repository.InsertAsync(customer);
            });
        }

   
        protected async Task<Document> CreateDocumentAsync(string customerApplicationId,   DocumentType type)
        {
            return await WithScopeAsync(async sp =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Document>>();

                var document = new Document(customerApplicationId,  "US", Guid.NewGuid().ToString(), type);

                return await repository.InsertAsync(document);
            });
        }

     
        protected async Task<DocumentAttachment> CreateDocumentAttachmentAsync(string documentId,   DocumentSide documentSide)
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
