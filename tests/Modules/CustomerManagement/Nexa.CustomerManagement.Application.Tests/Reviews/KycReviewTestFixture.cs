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

namespace Nexa.CustomerManagement.Application.Tests.Reviews
{
    public class KycReviewTestFixture : CustomerManagementTestFixture
    {
        protected Faker Faker { get; }
       
        protected IKYCProvider KycProvider { get; set; }
        public KycReviewTestFixture()
        {
            Faker = new Faker();
            KycProvider = ServiceProvider.GetRequiredService<IKYCProvider>();
        }

        protected override async Task InitializeAsync(IServiceProvider services)
        {
            await base.InitializeAsync(services);

            await TestHarness.Start();
        }

        protected override async Task ShutdownAsync(IServiceProvider services)
        {
            await base.InitializeAsync(services);

            await TestHarness.Stop();
        }

        protected async Task<KycReview> CreateDocumentReview(Customer customer, KycReviewStatus reviewStatus = KycReviewStatus.Pending, KycReviewOutcome reviewOutcome = KycReviewOutcome.Clear)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<KycReview>>();

                var kycCheck = await CreateKycCheck(customer, KYCCheckType.IdentityCheck, customer.Document!.Id, Guid.NewGuid().ToString());

                var kycReview = new KycReview(customer.Id, kycCheck.Id, kycCheck.LiveVideoId);

                if (reviewStatus == KycReviewStatus.Completed)
                    kycReview.Complete(reviewOutcome);

                return await repository.InsertAsync(kycReview);
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
                    Faker.PickRandom<Gender>()
                    
                    );

                customer.UpdateInfo(info);

                customer.UpdateAddress(address);

                var client = await CreateKycClient(customer);

                customer.AddKycCustomerId(client.Id);

                return await repository.InsertAsync(customer);           
            });
        }

        
        protected async Task<Customer> CreateDocumentAsync(string customerId, DocumentType type, string? issuingCountry = null, DocumentVerificationStatus verificationState = DocumentVerificationStatus.Pending)
        {
            return await WithScopeAsync(async sp =>
            {
                var repository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var customer = await repository.SingleAsync(x => x.Id == customerId);

                var kycDocument = await CreateKycDocument(customer.KycCustomerId!, type);

                var docuemnt = Document.Create(type, issuingCountry, kycDocument.Id);

                customer.UpdateDocument(docuemnt);

                if(verificationState != DocumentVerificationStatus.Pending)
                {
                    var reviewStatus = (verificationState == DocumentVerificationStatus.Verified
                      || verificationState == DocumentVerificationStatus.Rejected) ? KycReviewStatus.Completed : KycReviewStatus.Pending;

                    var reviewOutCome = verificationState == DocumentVerificationStatus.Verified ? KycReviewOutcome.Clear : KycReviewOutcome.Rejected;

                    var review = await CreateDocumentReview(customer);

                    customer.Document!.MarkAsProcessing(review.Id);

                    if (verificationState == DocumentVerificationStatus.Verified)
                    {
                        customer.Document!.MarkAsVerified();
                    }

                    if (verificationState == DocumentVerificationStatus.Rejected)
                    {
                        customer.Document!.MarkAsRejected();
                    }
                }

                await repository.UpdateAsync(customer);

                return customer;
            });
        }

        protected async Task<Customer> CreateDocumentAttachment(string customerId , DocumentSide documentSide)
        {
            return await WithScopeAsync(async (sp) =>
            {
                var customerRepository = sp.GetRequiredService<ICustomerManagementRepository<Customer>>();

                var customer = await customerRepository.SingleAsync(x => x.Id == customerId);

                var attachment = new DocumentAttachment(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), 6555, "img/png", documentSide);

                var kycAttachment = await CreateKycDocumentAttachment(customer.Document!.KycDocumentId, attachment.FileName, attachment.Side);

                attachment.AddKycExternalId(kycAttachment.Id);

                customer.Document.AddAttachment(attachment);

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
                IssuingCountry = type != DocumentType.Passport ? "US" : null
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
                };

            };

            if(customer.Address != null)
            {
                request.Address = customer.Address;
            }

            var kycClient = await KycProvider.CreateClientAsync(request);

            return kycClient;
        }

        protected async Task<KYCCheck> CreateKycCheck(Customer customer , KYCCheckType kYCCheckType , string? documentId = null , string? liveVideoId = null)
        {
            var request = new KYCCheckRequest
            {
                ClientId = customer.KycCustomerId!,
                DocumentId = documentId,
                LiveVideoId = liveVideoId,
                Type = kYCCheckType
            };

            return await KycProvider.CreateCheckAsync(request);
        }

    }
}
