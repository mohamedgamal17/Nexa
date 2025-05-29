using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Documents.Commands.UploadDocumentAttachment;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Documents;
namespace Nexa.CustomerManagement.Application.Tests.Documents.Commands
{
    [TestFixture]
    public class UploadDocumentAttachmentCommandHandlerTests : DocumentTestFixture
    {

        protected ICustomerManagementRepository<Document> DocumentRepositorty { get; }

        protected ICustomerManagementRepository<DocumentAttachment> DocumentAttachmentRepository { get; }
        public UploadDocumentAttachmentCommandHandlerTests()
        {
            DocumentRepositorty = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Document>>();
            DocumentAttachmentRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<DocumentAttachment>>();
        }


        [Test]
        public async Task Should_upload_document_attachment()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreatePendingDocumentAsync(fakeCustomer.Id, userId, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());

            var command = new UploadDocumentAttachmentCommand
            {
                DocumentId = fakeDocument.Id,
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var documentAttachment = await DocumentAttachmentRepository.SingleOrDefaultAsync(x => x.Id == result.Value!.Id);

            documentAttachment.Should().NotBeNull();

            documentAttachment!.Side.Should().Be(command.Side);


        }

        [Test]
        public async Task Should_failure_while_uploading_document_attachment_when_user_is_not_authorized()
        {
            var fakeCustomer = await CreateCustomerAsync();

            var fakeDocument = await CreatePendingDocumentAsync(fakeCustomer.Id, fakeCustomer.UserId, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());

            var command = new UploadDocumentAttachmentCommand
            {
                DocumentId = fakeDocument.Id,
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };


            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }

        [Test]
        public async Task Should_failure_while_uploading_document_attachment_when_user_is_not_own_the_document()
        {
            AuthenticationService.Login();

            var fakeCustomer = await CreateCustomerAsync();

            var fakeDocument = await CreatePendingDocumentAsync(fakeCustomer.Id, fakeCustomer.UserId, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());


            var command = new UploadDocumentAttachmentCommand
            {
                DocumentId = fakeDocument.Id,
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(ForbiddenAccessException));
        }

        [Test]
        public async Task Should_failure_while_uploading_document_attachment_when_document_is_already_in_processing()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreateActiveDocumentAsync(fakeCustomer.Id, userId, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());

            var command = new UploadDocumentAttachmentCommand
            {
                DocumentId = fakeDocument.Id,
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [Test]
        public async Task Should_failure_while_uploading_document_attachment_when_document_is_already_approved()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreateApprovedDocumentAsync(fakeCustomer.Id, userId, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());

            var command = new UploadDocumentAttachmentCommand
            {
                DocumentId = fakeDocument.Id,
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));

        }

        private async Task<IFormFile> PrepareImageData()
        {
            var stream = await Resource.LoadImageAsStream();

            var file = new FormFile(stream, 0, stream.Length, Guid.NewGuid().ToString(), $"{Guid.NewGuid().ToString()}.jpg")
            {
                Headers = new HeaderDictionary(),
                ContentType = "image/jpg"
            };

            return file;
        } 
    }
}
