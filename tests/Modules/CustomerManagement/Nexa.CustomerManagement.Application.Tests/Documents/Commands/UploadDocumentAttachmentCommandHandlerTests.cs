using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Documents.Commands.UploadDocumentAttachment;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Enums;
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

            var fakeCustomerApplication = await CreateCustomerApplicationAsync(fakeCustomer.Id);

            var fakeDocument = await CreateDocumentAsync(fakeCustomerApplication.Id, Faker.PickRandom<DocumentType>());

            var command = new UploadDocumentAttachmentCommand
            {
                CustomerApplicationId = fakeCustomerApplication.Id,
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

            var command = new UploadDocumentAttachmentCommand
            {
                CustomerApplicationId = Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid().ToString(),
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }

        [Test]
        public async Task Should_failure_while_uploading_document_attachment_when_user_is_not_own_customer_application()
        {
            AuthenticationService.Login();
            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var owenedCustomer = await CreateCustomerAsync(userId);

            var unownedCustomer = await CreateCustomerAsync();

            var fakeApplication = await CreateCustomerApplicationAsync(unownedCustomer.Id);

            var fakeDocument = await CreateDocumentAsync(fakeApplication.Id, DocumentType.Passport);


            var command = new UploadDocumentAttachmentCommand
            {
                CustomerApplicationId = fakeApplication.Id,
                DocumentId = fakeDocument.Id,
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(ForbiddenAccessException));
        }

        [Test]
        public async Task Should_failure_while_uploading_document_attachment_when_customer_application_not_exist()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            await CreateCustomerAsync(userId);


            var command = new UploadDocumentAttachmentCommand
            {
                CustomerApplicationId = Guid.NewGuid().ToString(),
                DocumentId = Guid.NewGuid().ToString(),
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException));

        }

        [Test]
        public async Task Should_failure_while_uploading_document_attachment_when_document_is_not_exist()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeApplication = await CreateCustomerApplicationAsync(fakeCustomer.Id);

            var command = new UploadDocumentAttachmentCommand
            {
                CustomerApplicationId = fakeApplication.Id,
                DocumentId = Guid.NewGuid().ToString(),
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException));

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
