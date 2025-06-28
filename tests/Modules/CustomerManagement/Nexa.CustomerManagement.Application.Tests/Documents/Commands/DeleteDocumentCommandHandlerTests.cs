using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Documents.Commands.DeleteDocument;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Enums;

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
        public async Task Should_failure_while_deleting_document_when_user_is_not_authorized()
        {
            var command = new DeleteDocumentCommand
            {
                DocumentId = Guid.NewGuid().ToString()
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }

        [Test]
        public async Task Should_failure_while_deleting_document_when_user_is_not_own_the_document()
        {
            AuthenticationService.Login();

            var fakeCustomer = await CreateCustomerAsync();

            var fakeDocument = await CreateDocumentAsync(fakeCustomer.Id, "US", Guid.NewGuid().ToString(), Faker.PickRandom<DocumentType>());

            var command = new DeleteDocumentCommand
            {
                DocumentId = fakeDocument.Id
            };

            var result = await Mediator.Send(command);

            result.ShoulBeFailure(typeof(ForbiddenAccessException));
        }
    }
}
