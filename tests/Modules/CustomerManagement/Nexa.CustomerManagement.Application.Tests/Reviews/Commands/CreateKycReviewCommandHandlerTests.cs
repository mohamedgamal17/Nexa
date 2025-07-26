using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.NewFolder;
using Nexa.CustomerManagement.Application.Reviews.Commands;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Reviews;
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

        [Test]
        public async Task Should_create_customer_info_kyc_review()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            await CreateCustomerInfo(fakeCustomer.Id);

            var command = new CreateKycReviewCommand
            {
                Type = KycReviewType.Info
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            var kycReview = await KycReviewRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id);

            kycReview.Should().NotBeNull();

            kycReview!.AssertKycReview(command, customer);

            customer.Info!.KycReviewId.Should().Be(kycReview!.Id);

            customer.Info!.State.Should().Be(VerificationState.Processing);

            result.Value!.AssertKycReviewDto(kycReview);
        }

        [TestCase(DocumentType.Passport)]
        [TestCase(DocumentType.DrivingLicense)]
        public async Task Should_create_document_kyc_review(DocumentType documentType)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            await CreateCustomerInfo(fakeCustomer.Id, VerificationState.Processing);

            string? issuingCountry = documentType != DocumentType.Passport ? "US" : null;

            fakeCustomer = await CreateDocumentAsync(fakeCustomer.Id, documentType, issuingCountry);

            await CreateDocumentAttachment(fakeCustomer.Id, DocumentSide.Front);

            if (fakeCustomer.Document!.RequireBothSides())
            {
                await CreateDocumentAttachment(fakeCustomer.Id, DocumentSide.Back);
            }


            var command = new CreateKycReviewCommand
            {
                Type = KycReviewType.Document,
                KycLiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            var kycReview = await KycReviewRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id);

            kycReview.Should().NotBeNull();

            kycReview!.AssertKycReview(command, customer);

            customer.Document!.KycReviewId.Should().Be(kycReview!.Id);

            customer.Document!.State.Should().Be(VerificationState.Processing);

            result.Value!.AssertKycReviewDto(kycReview);
        }

        [Test]
        public async Task Should_failure_while_creating_kyc_review_when_user_is_not_authorized()
        {
            var command = new CreateKycReviewCommand
            {
                Type = KycReviewType.Document,
                KycLiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }

        [Test]
        public async Task Should_failure_while_creating_kyc_review_when_customer_is_not_created()
        {
            AuthenticationService.Login();

            var command = new CreateKycReviewCommand
            {
                Type = KycReviewType.Document,
                KycLiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException));
        }

        [Test]
        public async Task Should_failure_while_creating_kyc_review_when_customer_info_is_not_completed()
        {
            AuthenticationService.Login();

            var userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            var command = new CreateKycReviewCommand
            {
                KycLiveVideoId = Guid.NewGuid().ToString(),
                Type = KycReviewType.Info
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [TestCase(VerificationState.Processing)]
        [TestCase(VerificationState.Verified)]
        public async Task Should_failure_while_creating_customer_info_kyc_review_when_info_is_in_invalid_state(VerificationState state)
        {
            AuthenticationService.Login();

            var userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            await CreateCustomerInfo(fakeCustomer.Id, state);

            var command = new CreateKycReviewCommand
            {
                Type = KycReviewType.Info
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }



        [Test]
        public async Task Should_failure_while_creating_document_kyc_review_when_document_is_not_created()
        {
            AuthenticationService.Login();

            var userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            await CreateCustomerInfo(fakeCustomer.Id);

            var command = new CreateKycReviewCommand
            {
                KycLiveVideoId = Guid.NewGuid().ToString(),
                Type = KycReviewType.Document
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [Test]
        public async Task Should_failure_while_creating_document_kyc_review_when_customer_info_is_not_reviewed_or_accepted()
        {
            AuthenticationService.Login();

            var userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            await CreateCustomerInfo(fakeCustomer.Id);

            await CreateDocumentAsync(fakeCustomer.Id, DocumentType.Passport);

            var command = new CreateKycReviewCommand
            {
                KycLiveVideoId = Guid.NewGuid().ToString(),
                Type = KycReviewType.Document
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [TestCase(DocumentType.Passport)]
        [TestCase(DocumentType.DrivingLicense)]
        public async Task Should_failure_while_creating_document_kyc_review_when_document_dose_not_have_require_attachments(DocumentType documentType)
        {
            AuthenticationService.Login();

            var userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            await CreateCustomerInfo(fakeCustomer.Id, VerificationState.Processing);

            fakeCustomer = await CreateDocumentAsync(fakeCustomer.Id, documentType);

            if (fakeCustomer.Document!.RequireBothSides())
            {
                await CreateDocumentAttachment(fakeCustomer.Id, DocumentSide.Front);
            }

            var command = new CreateKycReviewCommand
            {
                KycLiveVideoId = Guid.NewGuid().ToString(),
                Type = KycReviewType.Document
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [TestCase(VerificationState.Processing)]
        [TestCase(VerificationState.Verified)]
        public async Task Should_failure_while_creating_document_kyc_review_when_document_dose_not_have_valid_state(VerificationState state)
        {
            AuthenticationService.Login();

            var userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            await CreateCustomerInfo(fakeCustomer.Id, VerificationState.Processing);

            await CreateDocumentAsync(fakeCustomer.Id, DocumentType.Passport, verificationState: state);


            var command = new CreateKycReviewCommand
            {
                KycLiveVideoId = Guid.NewGuid().ToString(),
                Type = KycReviewType.Document
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

    }
}
