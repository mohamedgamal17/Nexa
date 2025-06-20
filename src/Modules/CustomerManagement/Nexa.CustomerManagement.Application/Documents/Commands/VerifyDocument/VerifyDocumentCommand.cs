using MediatR;
using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.Documents.Dtos;
using System.Net.Mail;

namespace Nexa.CustomerManagement.Application.Documents.Commands.VerifyDocument
{
    [Authorize]
    public class VerifyDocumentCommand : ICommand<DocumentDto>
    {
        public string DocumentId { get; set; }
    }
}
