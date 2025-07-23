using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Documents.Commands.VerifyDocument;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Tests.Documents.Commands
{
    [TestFixture]
    public class VerifiyDocumentCommandHandlerTests : DocumentTestFixture
    {
        protected ICustomerManagementRepository<Document> DocumentRepository { get; }

        public VerifiyDocumentCommandHandlerTests()
        {
            DocumentRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Document>>();
        }

        [TestCase(DocumentType.Passport)]
        [TestCase(DocumentType.DrivingLicense)]
        public async Task Should_verifiy_customer_document(DocumentType documentType)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId, VerificationState.Verified);

            var fakeDocument = await CreateDocumentAsync(fakeCustomer, documentType);

            await CreateDocumentAttachmentAsync(fakeDocument.Id, DocumentSide.Front);

            if (fakeDocument.RequireBothSides())
            {
                await CreateDocumentAttachmentAsync(fakeDocument.Id, DocumentSide.Back);
            }

            var command = new VerifyDocumentCommand
            {
                DocumentId = fakeDocument.Id,
                LiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var document = await DocumentRepository.AsQuerable()
                .Include(x => x.Attachments)
                .SingleAsync(x => x.Id == fakeDocument.Id);

            document.State.Should().Be(VerificationState.InReview);

            result.Value!.AssertDocumentDto(document);
        }

        [Test]
        public async Task Should_failure_while_verifing_customer_document_when_user_is_not_authorized()
        {
            var command = new VerifyDocumentCommand
            {
                DocumentId = Guid.NewGuid().ToString(),
                LiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }

        [Test]
        public async Task Should_failure_while_verifing_customer_document_when_user_customer_is_not_exist()
        {
            AuthenticationService.Login();

            var command = new VerifyDocumentCommand
            {
                DocumentId = Guid.NewGuid().ToString(),
                LiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [Test]
        public async Task Should_failure_while_verfing_customer_document_when_document_is_not_exist()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var command = new VerifyDocumentCommand
            {
                DocumentId = Guid.NewGuid().ToString(),
                LiveVideoId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException));
        }

        [TestCase(DocumentType.Passport)]
        [TestCase(DocumentType.DrivingLicense)]
        public async Task Should_failure_while_verifing_customer_document_when_document_attachments_is_incomplete(DocumentType documentType)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var document = await CreateDocumentAsync(fakeCustomer, documentType);

            if (document.RequireBothSides())
            {
                await CreateDocumentAttachmentAsync(document.Id, DocumentSide.Front);
            }

            var command = new VerifyDocumentCommand
            {
                DocumentId = document.Id,
                LiveVideoId = Guid.NewGuid().ToString(),
            };

            var result = await Mediator.Send(command);
        }
    }
}
