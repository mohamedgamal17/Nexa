using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Documents.Commands.VerifyDocument
{
    [Authorize]
    public class VerifyDocumentCommand :  ICommand<DocumentDto>
    {
        public string DocumentId { get; set; }
        public string LiveVideoId { get; set; }
    }
}
