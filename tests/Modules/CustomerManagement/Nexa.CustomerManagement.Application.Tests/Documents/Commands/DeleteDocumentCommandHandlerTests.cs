using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Documents.Commands.DeleteDocument;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Application.Tests.Documents.Commands
{
    [TestFixture]
    public class DeleteDocumentCommandHandlerTests : DocumentTestFixture
    {
        protected ICustomerManagementRepository<Document> DocumentRepositorty { get; }


        public DeleteDocumentCommandHandlerTests()
        {
            DocumentRepositorty = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Document>>();
        }

        [Test]
        public async Task Should_delete_pending_document()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreatePendingDocumentAsync(fakeCustomer.Id, userId, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());

            var command = new DeleteDocumentCommand
            {
                Id = fakeDocument.Id
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var document = await  DocumentRepositorty.SingleOrDefaultAsync(x => x.Id == fakeDocument.Id);

            document.Should().BeNull();
        }


        [Test]
        public async Task Should_delete_rejected_document()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreateRejectedDocumentAsync(fakeCustomer.Id, userId, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());

            var command = new DeleteDocumentCommand
            {
                Id = fakeDocument.Id
            };

            var result = await Mediator.Send(command);

            result.ShouldBeSuccess();

            var document = await DocumentRepositorty.SingleOrDefaultAsync(x => x.Id == fakeDocument.Id);

            document.Should().BeNull();
        }

        [Test]
        public async Task Should_failure_while_deleting_document_when_user_is_not_authorized()
        {
            var command = new DeleteDocumentCommand
            {
                Id = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }

        [Test]
        public async Task Should_failure_while_deleting_document_when_user_is_not_own_the_document()
        {
            AuthenticationService.Login();

            var fakeCustomer = await CreateCustomerAsync();

            var fakeDocument = await CreatePendingDocumentAsync(fakeCustomer.Id, fakeCustomer.UserId, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());

            var command = new DeleteDocumentCommand
            {
                Id = fakeDocument.Id
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(ForbiddenAccessException));
        }

        [Test]
        public async Task Should_failure_while_deleting_document_when_document_is_already_in_active_processing()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreateActiveDocumentAsync(fakeCustomer.Id, userId, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());

            var command = new DeleteDocumentCommand
            {
                Id = fakeDocument.Id
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [Test]
        public async Task Should_failure_while_deleting_document_when_document_is_already_in_approved()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeDocument = await CreateApprovedDocumentAsync(fakeCustomer.Id, userId, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());

            var command = new DeleteDocumentCommand
            {
                Id = fakeDocument.Id
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(BusinessLogicException));
        }
    }
}
