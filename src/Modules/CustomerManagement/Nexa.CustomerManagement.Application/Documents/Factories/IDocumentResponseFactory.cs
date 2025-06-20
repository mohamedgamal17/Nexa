using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Documents.Factories
{
    public interface IDocumentResponseFactory : IResponseFactory<Document, DocumentDto>
    {
    }
}
