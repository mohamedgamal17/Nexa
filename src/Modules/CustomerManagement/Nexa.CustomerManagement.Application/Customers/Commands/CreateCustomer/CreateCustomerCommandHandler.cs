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
using Nexa.Integrations.Baas.Abstractions.Contracts.Customers;
using Nexa.Integrations.Baas.Abstractions.Services;
namespace Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IApplicationRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ISecurityContext _securityContext;
        private readonly ICustomerResponseFactory _customerResponseFactory;
        private readonly IKYCProvider _kycProvider;
        private readonly IBaasCustomerService _baasCustomerService;
        public CreateCustomerCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ISecurityContext securityContext, ICustomerResponseFactory customerResponseFactory, IKYCProvider kycProvider, IBaasCustomerService baasCustomerService)
        {
            _customerRepository = customerRepository;
            _securityContext = securityContext;
            _customerResponseFactory = customerResponseFactory;
            _kycProvider = kycProvider;
            _baasCustomerService = baasCustomerService;
        }

        public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var userId = _securityContext.User!.Id;

            var userAlreadHasCustomer = await _customerRepository.AnyAsync(x => x.UserId == userId);

            if (userAlreadHasCustomer)
            {
                return new BusinessLogicException(CustomerErrorConsts.UserAlreadyHasCustomer);
            }

            var customer = new Customer(userId, request.PhoneNumber, request.EmailAddress);

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

            await AttachBaasCustomer(customer);

            await AttachKycCustomer(customer);

            await _customerRepository.InsertAsync(customer);

            var result = await _customerRepository.SingleAsync(x => x.Id == customer.Id);

            return await _customerResponseFactory.PrepareDto(result);
        }


        private async Task AttachBaasCustomer(Customer customer)
        {
            var baasRequest = PrepareBaasCustoemrRequest(customer);

            var baasCustomer = await _baasCustomerService.CreateCustomerAsync(baasRequest);

            customer.AddFintechCustomerId(baasCustomer.Id);
        }

        private async Task AttachKycCustomer(Customer customer)
        {
            var kycRequest = PrepareKycClientRequest(customer);

            var kycCustomer = await _kycProvider.CreateClientAsync(kycRequest);

            customer.AddKycCustomerId(kycCustomer.Id);
        }
        private KYCClientRequest PrepareKycClientRequest(Customer customer)
        {
            var request = new KYCClientRequest
            {
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber,
                Info = new KYCClientInfo
                {
                    FirstName = customer.Info.FirstName,
                    LastName = customer.Info.LastName,
                    BirthDate = customer.Info.BirthDate,
                    Gender = customer.Info.Gender,
                    Address = customer.Info.Address
                }
            };

            return request;
        }

        private CreateBaasCustomerRequest PrepareBaasCustoemrRequest(Customer customer)
        {
            var request = new CreateBaasCustomerRequest
            {
                FirstName = customer.Info!.FirstName,
                LastName = customer.Info!.LastName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.EmailAddress
            };

            return request;
        }

    }
}
