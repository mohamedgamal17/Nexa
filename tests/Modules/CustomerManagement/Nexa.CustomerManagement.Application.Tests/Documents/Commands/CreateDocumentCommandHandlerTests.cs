using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Nexa.Application.Tests.Extensions;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.CustomerManagement.Application.Documents.Commands.CreateDocument;
using Nexa.CustomerManagement.Application.Tests.Assertions;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Tests.Documents.Commands
{
    [TestFixture]
    public class CreateDocumentCommandHandlerTests : DocumentTestFixture
    {
        protected ICustomerManagementRepository<Document> DocumentRepositorty { get; }


        public CreateDocumentCommandHandlerTests()
        {
            DocumentRepositorty = ServiceProvider.GetRequiredService<ICustomerManagementRepository<Document>>();
        }

        [Test]
        public async Task Should_create_document()
        {
            AuthenticationService.Login();

            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var fakeCustomerApplication = await CreateCustomerApplicationAsync(fakeCustomer.Id);

            var command = new CreateDocumentCommand
            {
                CustomerApplicationId = fakeCustomerApplication.Id,
                IssuingCountry = "US",
                Type = Faker.PickRandom<DocumentType>()
            };

            var response = await Mediator.Send(command);

            response.ShouldBeSuccess();

            var document = await DocumentRepositorty.SingleOrDefaultAsync(x => x.Id == response.Value!.Id);

            document.Should().NotBeNull();

            document!.AssertDocument(command, fakeCustomerApplication.Id);

            response.Value!.AssertDocumentDto(document!);
        }

        [Test]
        public async Task Should_failure_while_creating_document_when_user_is_not_authenticated()
        {
            var command = new CreateDocumentCommand
            {
                CustomerApplicationId = Guid.NewGuid().ToString(),
                IssuingCountry = "US",
                Type = Faker.PickRandom<DocumentType>()
            };

            var response = await Mediator.Send(command);

            response.ShoulBeFailure(typeof(UnauthorizedAccessException));
        }


        [Test]
        public async Task Should_failure_while_creating_document_when_customer_is_not_created()
        {
            AuthenticationService.Login();

            var commadn = new CreateDocumentCommand
            {
                CustomerApplicationId = Guid.NewGuid().ToString(),
                IssuingCountry = "US",
                Type = Faker.PickRandom<DocumentType>()
            };

            var response = await Mediator.Send(commadn);

            response.ShoulBeFailure(typeof(BusinessLogicException));
        }

        [Test]
        public async Task Should_failure_while_creating_document_when_customer_application_is_not_created()
        {
            AuthenticationService.Login();


            string userId = AuthenticationService.GetCurrentUser()!.Id;

            var fakeCustomer = await CreateCustomerAsync(userId);

            var command = new CreateDocumentCommand
            {
                CustomerApplicationId = Guid.NewGuid().ToString(),
                IssuingCountry = "US",
                Type = Faker.PickRandom<DocumentType>()
            };

            var response = await Mediator.Send(command);

            response.ShoulBeFailure(typeof(EntityNotFoundException));
        }
    }
}
