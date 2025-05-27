using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.CustomerManagement.Application.KYC.Dtos;
using Nexa.CustomerManagement.Domain.KYC;
namespace Nexa.CustomerManagement.Application.KYC.Commands.CreateKYCDocument
{
    [Authorize]
    public class CreateKYCDocumentCommand  : ICommand<KYCDocumentDto>
    {
        public KYCDocumentType Type { get; set; }
    }
}
