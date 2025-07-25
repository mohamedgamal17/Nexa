using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Documents.Commands.CreateDocument;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Tests.Documents.Commands
{
    [TestFixture]
    public class CreateDocumentCommandHandlerTests : DocumentTestFixture
    {
        protected ICustomerManagementRepository<Document> DocumentRepositorty { get; }


        public CreateDocumentCommandHandlerTests()
        {
            DocumentRepositorty = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Document>>();
        }

        [Test]
        public async Task Should_create_document()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId, VerificationState.Verified);

            var command = new CreateDocumentCommand
            {
                Type = Faker.PickRandom<DocumentType>()
            };

            var response = await Mediator.Send(command);

            response.ShouldBeSuccess();

            var document = await DocumentRepositorty.SingleOrDefaultAsync(x => x.Id == response.Value!.Id);

            document.Should().NotBeNull();

            document!.AssertDocument(command, fakeCustomer.Id);

            response.Value!.AssertDocumentDto(document!);
        }

        [Test]
        public async Task Should_create_document_whith_external_kyc_document_id_from_sdk_client()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId, VerificationState.Verified);

            var fakeKycDocument = await CreateKycDocument(fakeCustomer.KycCustomerId!, DocumentType.Passport);

            var fakekycAttachment = await CreateKycDocumentAttachment(fakeKycDocument.Id, Guid.NewGuid().ToString(), DocumentSide.Front);

            var command = new CreateDocumentCommand
            {
                Type = DocumentType.Passport,
                KycDocumentId = fakeKycDocument.Id
            };

            var response = await Mediator.Send(command);

            response.ShouldBeSuccess();

            var document = await DocumentRepositorty
                .AsQuerable()
                .Include(x=> x.Attachments)
                .SingleOrDefaultAsync(x => x.Id == response.Value!.Id);

            document.Should().NotBeNull();

            document!.KycDocumentId.Should().Be(fakeKycDocument.Id);

            document.Attachments.Count.Should().Be(1);

            var attachment = document.Attachments.First();

            attachment.KYCExternalId.Should().Be(fakekycAttachment.Id);
            attachment.FileName.Should().Be(fakekycAttachment.FileName);
            attachment.Size.Should().Be(fakekycAttachment.Size);
            attachment.Side.Should().Be(fakekycAttachment.Side);
            attachment.ContentType.Should().Be(fakekycAttachment.ContentType);

            document!.AssertDocument(command, fakeCustomer.Id);

            response.Value!.AssertDocumentDto(document!);
        }

        [Test]
        public async Task Should_failure_while_creating_document_when_user_is_not_authenticated()
        {
            var command = new CreateDocumentCommand
            {
                Type = Faker.PickRandom<DocumentType>()
            };

            var response = await Mediator.Send(command);

            response.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }


        [Test]
        public async Task Should_failure_while_creating_document_when_customer_is_not_created()
        {
            AuthenticationService.Login();

            var commadn = new CreateDocumentCommand
            {
                Type = Faker.PickRandom<DocumentType>()
            };

            var response = await Mediator.Send(commadn);

            response.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [TestCase(VerificationState.Pending)]
        [TestCase(VerificationState.Processing)]
        [TestCase(VerificationState.Rejected)]

        public async Task Should_failure_while_creating_document_when_customer_info_state_is_not_verified(VerificationState state)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            var command = new CreateDocumentCommand
            {
                Type = Faker.PickRandom<DocumentType>()
            };

            var response = await Mediator.Send(command);

            response.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [Test]
        public async Task Should_failure_while_creating_document_when_user_dose_not_own_kyc_document_id_aquired_by_sdk_client()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId, VerificationState.Verified);

            var fakeKycDocument = await CreateKycDocument(Guid.NewGuid().ToString(), DocumentType.Passport);

            var command = new CreateDocumentCommand
            {
                Type = DocumentType.Passport,
                KycDocumentId = fakeKycDocument.Id
            };

            var response = await Mediator.Send(command);

            response.ShoulBeFailure(typeof(ForbiddenAccessException));
        }
    }
}
