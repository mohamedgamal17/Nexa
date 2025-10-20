using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Reviews.Commands.CreateKycReview;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.Reviews.Commands
{
    [TestFixture]
    public class CreateKycReviewCommandHandlerTests : KycReviewTestFixture
    {
        protected ICustomerManagementRepository<Customer> CustomerRepository { get; set; }
        protected ICustomerManagementRepository<KycReview> KycReviewRepository { get; set; }
        public CreateKycReviewCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
            KycReviewRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<KycReview>>();
        }

        [TestCase(DocumentType.Passport)]
        [TestCase(DocumentType.DrivingLicense)]
        public async Task Should_create_document_kyc_review(DocumentType documentType)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            string? issuingCountry = documentType != DocumentType.Passport ? "US" : null;

            fakeCustomer = await CreateDocumentAsync(fakeCustomer.Id, documentType, issuingCountry);

            await CreateDocumentAttachment(fakeCustomer.Id, DocumentSide.Front);

            if (fakeCustomer.Document!.RequireBothSides())
            {
                await CreateDocumentAttachment(fakeCustomer.Id, DocumentSide.Back);
            }


            var command = new CreateKycReviewCommand
            {

                KycLiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            var kycReview = await KycReviewRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id);

            kycReview.Should().NotBeNull();

            kycReview!.AssertKycReview(command, customer);

            customer.Document!.KycReviewId.Should().Be(kycReview!.Id);

            customer.Document!.Status.Should().Be(DocumentVerificationStatus.Processing);

            result.Value!.AssertKycReviewDto(kycReview);
        }

        [Test]
        public async Task Should_failure_while_creating_kyc_review_when_user_is_not_authorized()
        {
            var command = new CreateKycReviewCommand
            {
                KycLiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(NexaUnauthorizedAccessException), GlobalErrorConsts.UnauthorizedAccess);
        }

        [Test]
        public async Task Should_failure_while_creating_kyc_review_when_customer_is_not_created()
        {
            AuthenticationService.Login();

            var command = new CreateKycReviewCommand
            {

                KycLiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException), CustomerErrorConsts.CustomerNotExist);
        }

  


        [Test]
        public async Task Should_failure_while_creating_document_kyc_review_when_document_is_not_created()
        {
            AuthenticationService.Login();

            var userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var command = new CreateKycReviewCommand
            {
                KycLiveVideoId = Guid.NewGuid().ToString(),
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException),CustomerErrorConsts.DocumentNotExist);
        }


        [TestCase(DocumentType.Passport)]
        [TestCase(DocumentType.DrivingLicense)]
        public async Task Should_failure_while_creating_document_kyc_review_when_document_dose_not_have_require_attachments(DocumentType documentType)
        {
            AuthenticationService.Login();

            var userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            fakeCustomer = await CreateDocumentAsync(fakeCustomer.Id, documentType);

            var command = new CreateKycReviewCommand
            {
                KycLiveVideoId = Guid.NewGuid().ToString(),
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException), CustomerErrorConsts.IncompleteDocument);
        }

        [TestCase(DocumentVerificationStatus.Processing)]
        [TestCase(DocumentVerificationStatus.Verified)]
        public async Task Should_failure_while_creating_document_kyc_review_when_document_dose_not_have_valid_state(DocumentVerificationStatus state)
        {
            AuthenticationService.Login();

            var userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            fakeCustomer = await CreateDocumentAsync(fakeCustomer.Id, DocumentType.Passport, verificationState: state);

            await CreateDocumentAttachment(fakeCustomer.Id, DocumentSide.Front);

            if (fakeCustomer.Document!.RequireBothSides())
            {
                await CreateDocumentAttachment(fakeCustomer.Id, DocumentSide.Back);
            }

            var command = new CreateKycReviewCommand
            {
                KycLiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException),CustomerErrorConsts.InvalidDocumentVerificationState);
        }

    }
}
