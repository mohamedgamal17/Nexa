using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Documents.Dtos;
using Nexa.CustomerManagement.Domain.Documents;

namespace Nexa.CustomerManagement.Application.Documents.Commands.CreateDocument
{
    [Authorize]
    public class CreateDocumentCommand : ICommand<DocumentDto>
    {
        public DocumentType Type { get; set; }

        public string IssuingCountry { get; set; }
    }
}
