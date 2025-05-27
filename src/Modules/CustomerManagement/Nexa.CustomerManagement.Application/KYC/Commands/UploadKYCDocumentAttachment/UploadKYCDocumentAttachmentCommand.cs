using MediatR;
using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.KYC.Dtos;
using Nexa.CustomerManagement.Domain.KYC;

namespace Nexa.CustomerManagement.Application.KYC.Commands.UploadKYCDocumentAttachment
{
    [Authorize]
    public class UploadKYCDocumentAttachmentCommand : ICommand<KYCDocumentAttachementDto>
    {
        public string KYCDocumentId { get; set; }
        public string Data { get; set; }
        public DocumentSide Side { get; set; }
    }
}
