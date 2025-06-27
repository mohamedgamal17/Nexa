using Nexa.BuildingBlocks.Application.Factories;
using Nexa.CustomerManagement.Domain.CustomerApplications;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.CustomerApplications.Factories
{
    public interface ICustomerApplicationResponseFactory : IResponseFactory<CustomerApplication, CustomerApplicationDto>
    {
        Task<CustomerApplicationDto> PrepareFullCustomerApplicationDto(CustomerApplication application, KYCClient clinet); 
    }
}
