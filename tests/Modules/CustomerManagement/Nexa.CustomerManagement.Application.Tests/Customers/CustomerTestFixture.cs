using Bogus;
using Bogus.Extensions.UnitedStates;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Enums;
using Nexa.Integrations.Baas.Abstractions.Services;

namespace Nexa.CustomerManagement.Application.Tests.Customers
{
    public class CustomerTestFixture : CustomerManagementTestFixture
    {
        protected Faker Faker { get; }
        protected IKYCProvider KycProvider { get; set; }

        protected IBaasClientService FakeBaasClientService { get; set; }
        protected ITestHarness TestHarness { get; set; }
        public CustomerTestFixture()
        {
            Faker = new Faker();
            KycProvider = ServiceProvider.GetRequiredService<IKYCProvider>();
            FakeBaasClientService = ServiceProvider.GetRequiredService<IBaasClientService>();
            TestHarness = ServiceProvider.GetRequiredService<ITestHarness>();
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

                var address = Address.Create(
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
                    Faker.PickRandom<Gender>(),
                    address
                    );

                customer.UpdateInfo(info);

                var client = await CreateKycClient(customer);

                customer.AddKycCustomerId(client.Id);

                return await repository.InsertAsync(customer);
            });
        }
 
        protected async Task<Customer> CreateDocumentAsync(string customerId, DocumentType type , string? issuingCountry = null )
        {
            return await WithScopeAsync(async sp =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var customer = await repository.SingleAsync(x => x.Id == customerId);

                var kycDocument = await CreateKycDocument(customer.KycCustomerId!, type);

                var docuemnt = Document.Create(type, issuingCountry, kycDocument.Id);

                customer.UpdateDocument(docuemnt);

                await repository.UpdateAsync(customer);

                return customer;
            });
        }

        protected async Task<Customer> CreateDocumentWithAttachmentsAsync(string customerId, DocumentType type, string? issuingCountry = null)
        {
            return await WithScopeAsync(async sp =>
            {
               await CreateDocumentAsync(customerId, type, issuingCountry);

                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var customer = await repository.SingleAsync(x => x.Id == customerId);

                var frontKycAttachment = await CreateKycDocumentAttachment(customer.Document!.KycDocumentId, Guid.NewGuid().ToString(), DocumentSide.Front);

                var frontAttachment = new DocumentAttachment(
                        frontKycAttachment.FileName,
                        frontKycAttachment.Size,
                        frontKycAttachment.ContentType,
                        DocumentSide.Front
                    );

                customer.Document.AddAttachment(frontAttachment);

                if (customer.Document.RequireBothSides())
                {
                    var backKycAttachment = await CreateKycDocumentAttachment(customer.Document!.KycDocumentId, Guid.NewGuid().ToString(), DocumentSide.Back);

                    var backAttachment = new DocumentAttachment(
                            frontKycAttachment.FileName,
                            frontKycAttachment.Size,
                            frontKycAttachment.ContentType,
                            DocumentSide.Back
                        );

                    customer.Document.AddAttachment(backAttachment);

                }

                return await repository.UpdateAsync(customer);                
            });
        }

  
        protected async Task<Customer> ReviewCustomerDocument(string customerId)
        {
            return await WithScopeAsync(async sp =>
            {
                var customerRepository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var kycReviewRepository = sp.GetRequiredService<ICustomerManagementRepository<KycReview>>();

                var kycReview = new KycReview(customerId, Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), KycReviewType.Document);

                await kycReviewRepository.InsertAsync(kycReview);

                var customer = await customerRepository.SingleAsync(x => x.Id == customerId);

                customer.ReviewDocument(kycReview);

                await customerRepository.UpdateAsync(customer);

                return customer;
            });
        }

        protected async Task<Customer> AcceptCustomerDocument(string customerId)
        {
            return await WithScopeAsync(async sp =>
            {
                await ReviewCustomerDocument(customerId);

                var customerRepository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var kycReviewRepository = sp.GetRequiredService<ICustomerManagementRepository<KycReview>>();

                var customer = await customerRepository.SingleAsync(x => x.Id == customerId);

                var kycReview = await kycReviewRepository.SingleAsync(x => x.Id == customer.Document!.KycReviewId);

                kycReview.Complete(KycReviewOutcome.Clear);

                await kycReviewRepository.UpdateAsync(kycReview);

                customer.AcceptDocument();

                await customerRepository.UpdateAsync(customer);

                return customer;
            });
        }

        protected async Task<KYCDocument> CreateKycDocument(string clientId, DocumentType type)
        {
            var kycDocumentRequest = new KYCDocumentRequest
            {
                ClientId = clientId,
                Type = type,
                IssuingCountry=  type != DocumentType.Passport ? "US" : null
            };

            return await KycProvider.CreateDocumentAsync(kycDocumentRequest);
        }

        protected async Task<KYCDocumentAttachement> CreateKycDocumentAttachment(string kycDocumentId, string fileName, DocumentSide side)
        {

            var request = new KYCDocumentAttachmentRequest
            {
                FileName = fileName,
                Side = side
            };

            return await KycProvider.UploadDocumentAttachementAsync(kycDocumentId, request);
        }

        protected async Task<KYCClient> CreateKycClient(Customer customer)
        {
            var request = new KYCClientRequest
            {
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,

            };


            if (customer.Info != null)
            {
                request.Info = new KYCClientInfo
                {
                    FirstName = customer.Info.FirstName,
                    LastName = customer.Info.LastName,
                    BirthDate = customer.Info.BirthDate,
                    Gender = customer.Info.Gender,
                    Address = customer.Info.Address
                };

            };

            var kycClient = await KycProvider.CreateClientAsync(request);

            return kycClient;
        }

    }
}
