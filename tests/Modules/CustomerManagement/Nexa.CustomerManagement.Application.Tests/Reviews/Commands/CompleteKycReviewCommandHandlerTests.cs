using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.CustomerManagement.Application.Reviews.Commands.CompleteKycReview;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Reviews;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Tests.Reviews.Commands
{
    [TestFixture]
    public class CompleteKycReviewCommandHandlerTests : KycReviewTestFixture
    {
        protected ICustomerManagementRepository<Customer> CustomerRepository { get; }
        protected ICustomerManagementRepository<KycReview> KycReviewRepository { get; }

        public CompleteKycReviewCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
            KycReviewRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<KycReview>>();
        }


        [Test]
        public async Task Should_accept_document_kyc_review()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            fakeCustomer = await CreateDocumentAsync(fakeCustomer.Id, DocumentType.Passport, verificationState: DocumentVerificationStatus.Processing);

            var fakeReview = await KycReviewRepository.SingleAsync(x => x.Id == fakeCustomer.Document!.KycReviewId);

            var command = new CompleteKycReviewCommand
            {
                KycCheckId = fakeReview.KycCheckId,
                Outcome = KycReviewOutcome.Clear
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var kycReview = await KycReviewRepository.SingleAsync(x => x.Id == fakeReview.Id);
            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            kycReview.Status.Should().Be(KycReviewStatus.Completed);
            kycReview.Outcome.Should().Be(KycReviewOutcome.Clear);

            customer.Document!.Status.Should().Be(DocumentVerificationStatus.Verified);
            customer.Status.Should().Be(CustomerStatus.Verified);
        }


        [Test]
        public async Task Should_reject_document_kyc_review()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            fakeCustomer = await CreateDocumentAsync(fakeCustomer.Id, DocumentType.Passport, verificationState: DocumentVerificationStatus.Processing);

            var fakeReview = await KycReviewRepository.SingleAsync(x => x.Id == fakeCustomer.Document!.KycReviewId);

            var command = new CompleteKycReviewCommand
            {
                KycCheckId = fakeReview.KycCheckId,
                Outcome = KycReviewOutcome.Rejected
            };
            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();
            var kycReview = await KycReviewRepository.SingleAsync(x => x.Id == fakeReview.Id);
            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            kycReview.Status.Should().Be(KycReviewStatus.Completed);

            kycReview.Outcome.Should().Be(KycReviewOutcome.Rejected);

            customer.Document!.Status.Should().Be(DocumentVerificationStatus.Rejected);
        }

    }
}
