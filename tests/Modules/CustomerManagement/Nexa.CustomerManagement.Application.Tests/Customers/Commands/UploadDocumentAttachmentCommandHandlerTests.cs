using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain;
using Microsoft.Extensions.DependencyInjection;
using Bogus;
using Nexa.CustomerManagement.Application.Customers.Commands.UploadDocumentAttachment;
using Nexa.CustomerManagement.Shared.Enums;
using Microsoft.AspNetCore.Http;
using Nexa.Application.Tests.Extensions;
using FluentAssertions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Shared.Consts;
namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    [TestFixture]
    public class UploadDocumentAttachmentCommandHandlerTests : CustomerTestFixture
    {

        protected ICustomerManagementRepository<Customer> CustomerRepository { get; }

        public UploadDocumentAttachmentCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [Test]
        public async Task Should_upload_document_attachment()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreateDocumentAsync(fakeCustomer.Id, DocumentType.Passport);

            var command = new UploadDocumentAttachmentCommand
            {
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            var document = customer.Document!;

            document.Attachments.Count.Should().BeGreaterThan(0);

            var documentAttachment = document.Attachments.First();

            documentAttachment!.Side.Should().Be(command.Side);
        }

        [Test]
        public async Task Should_failure_while_uploading_document_attachment_when_user_is_not_authorized()
        {

            var command = new UploadDocumentAttachmentCommand
            {
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(NexaUnauthorizedAccessException), GlobalErrorConsts.UnauthorizedAccess);
        }

        [Test]
        public async Task Should_failure_while_uploading_document_attachment_when_customer_is_not_exist()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var command = new UploadDocumentAttachmentCommand
            {
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(EntityNotFoundException),CustomerErrorConsts.CustomerNotExist);
        }

        [Test]
        public async Task Should_failure_while_uploading_document_attachment_when_document_is_not_created()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            var command = new UploadDocumentAttachmentCommand
            {
                Data = await PrepareImageData(),
                Side = Faker.PickRandom<DocumentSide>()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException), CustomerErrorConsts.DocumentNotExist);
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
