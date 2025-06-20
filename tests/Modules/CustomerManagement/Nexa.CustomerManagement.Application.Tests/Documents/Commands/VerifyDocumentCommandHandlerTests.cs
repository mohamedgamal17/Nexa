using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Nexa.CustomerManagement.Application.Documents.Commands.VerifyDocument;
using Nexa.Application.Tests.Extensions;
using FluentAssertions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using System.Runtime.CompilerServices;

namespace Nexa.CustomerManagement.Application.Tests.Documents.Commands
{
    [TestFixture]
    public class VerifyDocumentCommandHandlerTests : DocumentTestFixture
    {
        protected ICustomerManagementRepository<Document> DocumentRepositorty { get; }
        protected ICustomerManagementRepository<DocumentAttachment> DocumentAttachmentRepository { get; }
        public VerifyDocumentCommandHandlerTests()
        {
            DocumentRepositorty = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Document>>();
            DocumentAttachmentRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<DocumentAttachment>>();
        }

        [Test]
        public async Task Should_verfiy_customer_document()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreatePendingDocumentAsync(fakeCustomer.Id, userId, "US", Guid.NewGuid().ToString(), DocumentType.Passport);

            await CreateDocumentAttachmentAsync(fakeDocument.Id, userId, DocumentSide.Front);

            await CreateDocumentAttachmentAsync(fakeDocument.Id, userId, DocumentSide.Back);

            var command = new VerifyDocumentCommand
            {
                DocumentId = fakeDocument.Id
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var document = await DocumentRepositorty.SingleAsync(x => x.Id == fakeDocument.Id);

            document.Status.Should().Be(DocumentStatus.Processing);
        }

        [Test]
        public async Task Should_failure_while_verifing_customer_document_when_user_is_not_authenticated()
        {
            var command = new VerifyDocumentCommand
            {
                DocumentId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }

        [Test]
        public async Task Should_failure_while_verifing_customer_document_when_user_dose_not_complete_customer_application()
        {
            AuthenticationService.Login();

            var command = new VerifyDocumentCommand
            {
                DocumentId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [Test]
        public async Task Should_failure_while_verifing_customer_document_when_doucment_is_not_exist()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var command = new VerifyDocumentCommand
            {
                DocumentId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException));
        }

        [Test]
        public async Task Should_failure_while_verifing_customer_document_when_user_dose_not_own_document()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreatePendingDocumentAsync(fakeCustomer.Id, Guid.NewGuid().ToString(), "US", Guid.NewGuid().ToString(), DocumentType.Passport);

            var command = new VerifyDocumentCommand
            {
                DocumentId = fakeDocument.Id
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(ForbiddenAccessException));
        }

        [Test]
        public async  Task Should_failure_while_verifing_customer_document_when_document_status_is_not_pending()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreateActiveDocumentAsync(fakeCustomer.Id, userId, "US", Guid.NewGuid().ToString(), DocumentType.Passport);

            var command = new VerifyDocumentCommand
            {
                DocumentId = fakeDocument.Id
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));

        }

        [Test]
        [TestCase(null)]
        [TestCase(DocumentSide.Front)]
        [TestCase(DocumentSide.Back)]
        public async Task Should_failure_while_verifing_customer_doucment_when_attachment_dose_not_contain_both_sides_or_null(DocumentSide? documentSide = null)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreatePendingDocumentAsync(fakeCustomer.Id, userId, "US", Guid.NewGuid().ToString(), DocumentType.Passport);

            if(documentSide != null)
            {
                await CreateDocumentAttachmentAsync(fakeDocument.Id, userId, documentSide.Value);
            }

            var command = new VerifyDocumentCommand
            {
                DocumentId = fakeDocument.Id
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }
    }
}
