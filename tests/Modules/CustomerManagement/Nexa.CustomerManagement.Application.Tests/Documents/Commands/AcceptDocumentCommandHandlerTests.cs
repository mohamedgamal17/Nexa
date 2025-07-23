using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.CustomerManagement.Application.Documents.Commands.AcceptDocument;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Tests.Documents.Commands
{
    [TestFixture]
    public class AcceptDocumentCommandHandlerTests : DocumentTestFixture
    {
        public ICustomerManagementRepository<Document> DocumentRepository { get; }

        public AcceptDocumentCommandHandlerTests()
        {
            DocumentRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Document>>();
        }

        [TestCase(DocumentType.Passport)]
        [TestCase(DocumentType.DrivingLicense)]
        public async Task Should_accept_document_verification(DocumentType documentType)
        {
            var fakeCustomer = await CreateCustomerAsync();

            var fakeDocument = await CreateDocumentAsync(fakeCustomer, documentType, VerificationState.InReview);

            await CreateDocumentAttachmentAsync(fakeDocument.Id, DocumentSide.Front);

            if (fakeDocument.RequireBothSides())
            {
                await CreateDocumentAttachmentAsync(fakeDocument.Id, DocumentSide.Back);
            }

            var command = new AcceptDocumentCommand
            {
                KycCustomeId = fakeCustomer.KycCustomerId!,
                KycDocumentId = fakeDocument.KYCExternalId!
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var document = await DocumentRepository
                .AsQuerable()
                .Include(c => c.Attachments)
                .SingleAsync(c => c.Id == fakeDocument.Id);

            document.State.Should().Be(VerificationState.Verified);
        }
    }
}
