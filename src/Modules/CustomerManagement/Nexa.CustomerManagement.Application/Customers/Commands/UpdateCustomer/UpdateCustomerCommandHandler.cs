using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.Integrations.Baas.Abstractions.Contracts.Clients;
using Nexa.Integrations.Baas.Abstractions.Services;
namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IApplicationRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly ISecurityContext _securityContext;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerResponseFactory _customerResponseFactory;
        private readonly IKYCProvider _kycProvider;
        private readonly IBaasCustomerService _baasCustomerService;

        public UpdateCustomerCommandHandler(ISecurityContext securityContext, ICustomerManagementRepository<Customer> customerRepository, ICustomerResponseFactory customerResponseFactory, IKYCProvider kycProvider, IBaasCustomerService baasCustomerService)
        {
            _securityContext = securityContext;
            _customerRepository = customerRepository;
            _customerResponseFactory = customerResponseFactory;
            _kycProvider = kycProvider;
            _baasCustomerService = baasCustomerService;
        }

        public async Task<Result<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
            {
                return new EntityNotFoundException(CustomerErrorConsts.CustomerNotExist);
            }


            var address = Domain.Customers.Address.Create(
                      request.Address.Country,
                      request.Address.City,
                      request.Address.State,
                      request.Address.StreetLine,
                      request.Address.PostalCode,
                      request.Address.ZipCode
                  );

            var info = CustomerInfo.Create(
                    request.FirstName,
                    request.LastName,
                    request.BirthDate,
                    request.Gender,
                    address
                );

            customer.UpdateInfo(info);

            await UpdateBaasCustomer(customer);

            await UpdateKycCustomerRequest(customer);
            
            await _customerRepository.UpdateAsync(customer);

            var result = await _customerRepository.SingleAsync(x => x.Id == customer.Id);

            return await _customerResponseFactory.PrepareDto(result);
        }

        private async Task UpdateBaasCustomer(Customer customer)
        {
            var request = PrepareBaasCustoemrRequest(customer);

            await _baasCustomerService.UpdateCustomerAsync(customer.FintechCustomerId!, request);
        }

        private async Task UpdateKycCustomerRequest(Customer customer)
        {
            var request = PrepareKycClientRequest(customer);

            await _kycProvider.UpdateClientAsync(customer.KycCustomerId!, request);
        }
        private UpdateBaasCustomerRequest PrepareBaasCustoemrRequest(Customer customer)
        {
            var request = new UpdateBaasCustomerRequest
            {
                FirstName = customer.Info!.FirstName,
                LastName = customer.Info!.LastName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.EmailAddress
            };

            return request;
        }
        private KYCClientRequest PrepareKycClientRequest(Customer customer)
        {
            var request = new KYCClientRequest
            {
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,
                Info = new KYCClientInfo
                {
                    FirstName = customer.Info!.FirstName,
                    LastName = customer.Info.LastName,
                    BirthDate = customer.Info.BirthDate,
                    Gender = customer.Info.Gender,
                    Address = customer.Info.Address
                }
            };

            return request;
        }
    }
}
