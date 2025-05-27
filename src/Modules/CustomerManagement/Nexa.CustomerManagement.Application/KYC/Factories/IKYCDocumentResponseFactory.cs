using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Application.KYC.Dtos;
using Nexa.CustomerManagement.Domain.KYC;

namespace Nexa.CustomerManagement.Application.KYC.Factories
{
    public interface IKYCDocumentResponseFactory : IResponseFactory<KYCDocument, KYCDocumentDto>
    {
    }
}
