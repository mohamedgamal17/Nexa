using Bogus;
using Bogus.Extensions.UnitedStates;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Tests.Documents
{
    public abstract class DocumentTestFixture : CustomerManagementTestFixture
    {
        protected Faker Faker { get; }

        protected IKYCProvider KycProvider {get;}
        protected DocumentTestFixture()
        {
            Faker = new Faker();
            KycProvider = ServiceProvider.GetRequiredService<IKYCProvider>();
        }

        protected async Task<Customer> CreateCustomerWithoutInfo(string? userId = null)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var customer = new Customer(userId ?? Guid.NewGuid().ToString(), Faker.Person.Phone, Faker.Person.Email);

                var client = await CreateKycClient(customer);

                customer.AddKycCustomerId(client.Id);

                return await repository.InsertAsync(customer);
            });
        }
        protected async Task<Customer> CreateCustomerAsync(string? userId = null)
        {

            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var customer = new Customer(userId ?? Guid.NewGuid().ToString(), Faker.Person.Phone, Faker.Person.Email);

                var address = Address.Create (
                        "US",
                        Faker.Address.City(),
                        Faker.Address.State(),
                        Faker.Address.StreetName(),
                        Faker.Person.Address.ZipCode,
                        Faker.Person.Address.ZipCode
                    );

                var info = CustomerInfo.Create(
                    Faker.Person.FirstName,
                    Faker.Person.LastName,
                    Faker.Person.DateOfBirth,
                    "US",
                    Faker.PickRandom<Gender>(),
                    Faker.Person.Ssn(),
                    address
                    );

                customer.UpdateInfo(info);

                var client = await CreateKycClient(customer);

                customer.AddKycCustomerId(client.Id);

                return await repository.InsertAsync(customer);
            });
        }

   

        protected async Task<Document> CreateDocumentAsync(Customer customer,   DocumentType type)
        {
            return await WithScopeAsync(async sp =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var docuemnt = new Document(type);

                var kycDocument = await CreateKycDocument(customer.KycCustomerId!, type);

                customer.AddDocument(docuemnt);

                await repository.UpdateAsync(customer);

                return docuemnt;
            });
        }

     
        protected async Task<DocumentAttachment> CreateDocumentAttachmentAsync(string documentId,   DocumentSide documentSide)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var documentRepository = sp.GetRequiredService<ICustomerManagementRepository<Document>>();

                var document = await documentRepository.SingleAsync(x => x.Id == documentId);

                var attachment = new DocumentAttachment(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 6555, "img/png", documentSide);

                var kycAttachment = await CreateKycDocumentAttachment(document.KYCExternalId!, attachment.FileName, attachment.Side);

                attachment.AddKycExternalId(kycAttachment.Id);

                document.AddAttachment(attachment);

                await documentRepository.UpdateAsync(document);

                return attachment;
            });
        }

        protected Task<KYCClient> CreateKycClient(Customer customer)
        {
            var request = new KYCClientRequest
            {
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,

            };

            if(customer.Info != null)
            {
                request.FirstName = customer.Info.FirstName;
                request.LastName = customer.Info.LastName;
                request.Gender = customer.Info.Gender;
                request.BirthDate = customer.Info.BirthDate;
                request.NationalIdentityNumber = customer.Info.IdNumber;
                request.SSN = customer.Info.IdNumber;
            };

            return KycProvider.CreateClientAsync(request);
        }

        protected async Task<KYCDocument> CreateKycDocument(string clientId, DocumentType type)
        {
            var kycDocumentRequest = new KYCDocumentRequest
            {
                ClientId = clientId,
                Type = type
            };

            return await KycProvider.CreateDocumentAsync(kycDocumentRequest);
        }

        protected async Task<KYCDocumentAttachement> CreateKycDocumentAttachment(string kycDocumentId , string fileName, DocumentSide side)
        {

            var request = new KYCDocumentAttachmentRequest
            {
                FileName = fileName,
                Side = side
            };

            return await KycProvider.UploadDocumentAttachementAsync(kycDocumentId, request);
        }
    }
}
