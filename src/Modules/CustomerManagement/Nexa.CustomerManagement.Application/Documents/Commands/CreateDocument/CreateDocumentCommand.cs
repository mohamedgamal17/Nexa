using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Documents.Commands.CreateDocument
{
    [Authorize]
    public class CreateDocumentCommand : ICommand<DocumentDto>
    {
        public string CustomerApplicationId { get; set; }
        public DocumentType Type { get; set; }

        public string IssuingCountry { get; set; }
    }
}
