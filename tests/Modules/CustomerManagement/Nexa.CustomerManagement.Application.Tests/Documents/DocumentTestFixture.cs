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

        protected async Task<CustomerApplication> CreateCustomerApplicationAsync(string customerId)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<CustomerApplication>>();

                var customerApplication = new CustomerApplication
                {
                    CustomerId = customerId,
                    KycExternalId = Guid.NewGuid().ToString(),
                    FirstName = Faker.Person.FirstName,
                    LastName = Faker.Person.LastName,
                    PhoneNumber = Faker.Person.Phone,
                    EmailAddress = Faker.Person.Email,
                    Nationality = "US",
                    Gender = Faker.Person.Gender == Bogus.DataSets.Name.Gender.Male ? Gender.Male : Gender.Female,
                    BirthDate = Faker.Person.DateOfBirth,
                    Address = new Address
                    {
                        Country = "US",
                        City = Faker.Person.Address.City,
                        State = Faker.Person.Address.State,
                        StreetLine1 = Faker.Person.Address.Street,
                        StreetLine2 = Faker.Person.Address.Street,
                        PostalCode = Faker.Person.Address.ZipCode,
                        ZipCode = Faker.Person.Address.ZipCode
                    },

                };

                return await repository.InsertAsync(customerApplication);
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
