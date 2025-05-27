using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;

namespace Nexa.CustomerManagement.Application.KYC.Commands.DeleteKYCDocument
{
    [Authorize]
    public class DeleteKYCDocumentCommand : ICommand
    {
        public string Id { get; set; }
    }
}
