//using FluentAssertions;
//using Microsoft.Extensions.DependencyInjection;
//using Nexa.Application.Tests.Extensions;
//using Nexa.BuildingBlocks.Domain.Exceptions;
//using Nexa.CustomerManagement.Application.Documents.Commands.DeleteDocument;
//using Nexa.CustomerManagement.Domain;
//using Nexa.CustomerManagement.Domain.Documents;
//using Nexa.CustomerManagement.Shared.Enums;

//namespace Nexa.CustomerManagement.Application.Tests.Documents.Commands
//{
//    [TestFixture]
//    public class DeleteDocumentCommandHandlerTests : DocumentTestFixture
//    {
//        protected ICustomerManagementRepository<Document> DocumentRepositorty { get; }


//        public DeleteDocumentCommandHandlerTests()
//        {
//            DocumentRepositorty = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Document>>();
//        }


//        [Test]
//        public async Task Should_delete_document()
//        {
//            AuthenticationService.Login();

//            string userId = AuthenticationService.GetCurrentUser()!.Id;

//            var fakeCustomer = await CreateCustomerAsync(userId);

//            var fakeApplication = await CreateCustomerApplicationAsync(fakeCustomer.Id);

//            var fakeDocument = await CreateDocumentAsync(fakeApplication.Id, DocumentType.DrivingLicense);

//            var command = new DeleteDocumentCommand
//            {
//                CustomerApplicationId = fakeApplication.Id,
//                DocumentId = fakeDocument.Id
//            };

//            var result = await Mediator.Send(command);

//            result.ShouldBeSuccess();

//            var document = await DocumentRepositorty.SingleOrDefaultAsync(x => x.Id == fakeDocument.Id);

//            document.Should().BeNull();
//        }
      
//        [Test]
//        public async Task Should_failure_while_deleting_document_when_user_is_not_authorized()
//        {
//            var command = new DeleteDocumentCommand
//            {
//                CustomerApplicationId = Guid.NewGuid().ToString(),
//                DocumentId = Guid.NewGuid().ToString()
//            };

//            var result = await Mediator.Send(command);

//            result.ShoulBeFailure(typeof(UnauthorizedAccessException));
//        }

//        [Test]
//        public async Task Should_failure_while_deleting_document_when_customer_is_not_created()
//        {
//            AuthenticationService.Login();

            
//            var command = new DeleteDocumentCommand
//            {
//                CustomerApplicationId = Guid.NewGuid().ToString(),
//                DocumentId = Guid.NewGuid().ToString()
//            };

//            var result = await Mediator.Send(command);

//            result.ShoulBeFailure(typeof(BusinessLogicException));
//        }

   

//        [Test]
//        public async Task Should_failure_while_deleting_document_when_customer_application_is_not_exist()
//        {
//            AuthenticationService.Login();

//            string userId = AuthenticationService.GetCurrentUser()!.Id;

//            await CreateCustomerAsync(userId);

//            var command = new DeleteDocumentCommand
//            {
//                CustomerApplicationId = Guid.NewGuid().ToString(),
//                DocumentId = Guid.NewGuid().ToString()
//            };

//            var result = await Mediator.Send(command);

//            result.ShoulBeFailure(typeof(EntityNotFoundException));
//        }

//        [Test]
//        public async Task Should_failure_while_deleting_document_when_does_not_own_customer_application()
//        {
//            AuthenticationService.Login();

//            string userId = AuthenticationService.GetCurrentUser()!.Id;

//            await CreateCustomerAsync(userId);

//            var unownedCustomer = await CreateCustomerAsync();

//            var fakeCustomerApplication = await CreateCustomerApplicationAsync(unownedCustomer.Id);

//            var fakeDocument = await CreateDocumentAsync(fakeCustomerApplication.Id, DocumentType.Passport);

//            var command = new DeleteDocumentCommand
//            {
//                CustomerApplicationId = fakeCustomerApplication.Id,
//                DocumentId = fakeDocument.Id
//            };

//            var result = await Mediator.Send(command);

//            result.ShoulBeFailure(typeof(ForbiddenAccessException));
//        }

//        [Test]
//        public async Task Should_failure_while_deleting_document_when_document_is_not_exist()
//        {
//            AuthenticationService.Login();

//            string userId = AuthenticationService.GetCurrentUser()!.Id;

//            var fakeCustomer = await CreateCustomerAsync(userId);

//            var fakeCustomerApplication = await CreateCustomerApplicationAsync(fakeCustomer.Id);


//            var command = new DeleteDocumentCommand
//            {
//                CustomerApplicationId = fakeCustomerApplication.Id,
//                DocumentId = Guid.NewGuid().ToString()
//            };

//            var result = await Mediator.Send(command);

//            result.ShoulBeFailure(typeof(EntityNotFoundException));
//        }

//    }
//}
