using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.CustomerManagement.Application.Documents.Commands.DeleteDocument
{
    [Authorize]
    public class DeleteDocumentCommand : ICommand
    {
        public string CustomerApplicationId { get; set; }
        public string DocumentId { get; set; }
    }
}
