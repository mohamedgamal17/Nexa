using MediatR;
using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateDocument
{
    [Authorize]
    public class UpdateDocumentCommand : ICommand<CustomerDto>
    {
        public string? KycDocumentId { get; set; }
        public DocumentType Type { get; set; }
        public string? IssuingCountry { get; set; }
    }
}
