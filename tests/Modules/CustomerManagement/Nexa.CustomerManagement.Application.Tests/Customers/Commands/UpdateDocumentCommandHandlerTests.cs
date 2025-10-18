using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.CustomerManagement.Application.Customers.Commands.UpdateDocument;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;
using FluentAssertions;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.CustomerManagement.Shared.Consts;
namespace Nexa.CustomerManagement.Application.Tests.Customers.Commands
{
    [TestFixture]
    public class UpdateDocumentCommandHandlerTests : CustomerTestFixture
    {

        protected ICustomerManagementRepository<Customer> CustomerRepository { get; }

        public UpdateDocumentCommandHandlerTests()
        {
            CustomerRepository = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Customer>>();
        }

        [TestCase(DocumentType.Passport)]
        [TestCase(DocumentType.DrivingLicense)]
        public async Task Should_create_document(DocumentType documentType)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);


            var command = new UpdateDocumentCommand
            {
                Type = documentType,
                IssuingCountry = documentType != DocumentType.Passport ? "US" : null
            };

            var response = await Mediator.Send(command);

            response.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            customer.Document.Should().NotBeNull();

            customer.Document!.AssertDocument(command);

            response.Value!.AssertCustomerDto(customer);
        }

        [TestCase(DocumentType.Passport)]
        [TestCase(DocumentType.DrivingLicense)]
        public async Task Should_create_document_whith_external_kyc_document_id_from_sdk_client(DocumentType documentType)
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeKycDocument = await CreateKycDocument(fakeCustomer.KycCustomerId!, documentType);

            var fakekycAttachment = await CreateKycDocumentAttachment(fakeKycDocument.Id, Guid.NewGuid().ToString(), DocumentSide.Front);

            var command = new UpdateDocumentCommand
            {
                Type = DocumentType.Passport,
                KycDocumentId = fakeKycDocument.Id,
                IssuingCountry = fakeKycDocument.IssuingCountry
            };

            var response = await Mediator.Send(command);

            response.ShouldBeSuccess();

            var customer = await CustomerRepository.SingleAsync(x => x.Id == fakeCustomer.Id);

            var document = customer.Document;


            document.Should().NotBeNull();

            document!.KycDocumentId.Should().Be(fakeKycDocument.Id);

            document.Attachments.Count.Should().Be(1);

            var attachment = document.Attachments.First();

            attachment.KycAttachmentId.Should().Be(fakekycAttachment.Id);
            attachment.FileName.Should().Be(fakekycAttachment.FileName);
            attachment.Size.Should().Be(fakekycAttachment.Size);
            attachment.Side.Should().Be(fakekycAttachment.Side);
            attachment.ContentType.Should().Be(fakekycAttachment.ContentType);

            document!.AssertDocument(command);

            response.Value!.AssertCustomerDto(customer!);
        }


        [Test]
        public async Task Should_failure_while_creating_document_when_user_is_not_authenticated()
        {
            var command = new UpdateDocumentCommand
            {
                Type = DocumentType.Passport
            };

            var response = await Mediator.Send(command);

            response.ShoulBeFailure(typeof(NexaUnauthorizedAccessException), GlobalErrorConsts.UnauthorizedAccess);
        }
        [Test]
        public async Task Should_failure_while_creating_document_when_customer_is_not_created()
        {
            AuthenticationService.Login();

            var command = new UpdateDocumentCommand
            {
                Type = DocumentType.Passport
            };

            var response = await Mediator.Send(command);

            response.ShoulBeFailure(typeof(EntityNotFoundException), CustomerErrorConsts.CustomerNotExist);
        }

        [Test]
        public async Task Should_failure_while_creating_document_when_customer_info_is_not_completed()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerWithoutInfo(userId);

            var command = new UpdateDocumentCommand
            {
                Type = DocumentType.Passport
            };

            var response = await Mediator.Send(command);

            response.ShoulBeFailure(typeof(BusinessLogicException),CustomerErrorConsts.IncompleteCustomerInfo);
        }

        [Test]
        public async Task Should_failure_while_creating_document_when_user_dose_not_own_kyc_document_id_aquired_by_sdk_client()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeKycDocument = await CreateKycDocument(Guid.NewGuid().ToString(), DocumentType.Passport);

            var command = new UpdateDocumentCommand
            {
                Type = DocumentType.Passport,
                KycDocumentId = fakeKycDocument.Id
            };

            var response = await Mediator.Send(command);

            response.ShoulBeFailure(typeof(ForbiddenAccessException),CustomerErrorConsts.DocumentNotOwned);
        }


    }
}
