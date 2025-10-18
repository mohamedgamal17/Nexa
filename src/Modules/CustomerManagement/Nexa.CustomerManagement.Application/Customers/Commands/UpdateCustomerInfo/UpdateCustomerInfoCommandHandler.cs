using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Consts;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Consts;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomerInfo
{
    public class UpdateCustomerInfoCommandHandler : IApplicationRequestHandler<UpdateCustomerInfoCommand, CustomerDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerResponseFactory _customerResponseFactory;
        private readonly ISecurityContext _securityContext;
        private readonly IKYCProvider _kycProvider;

        public UpdateCustomerInfoCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerResponseFactory customerResponseFactory, ISecurityContext securityContext, IKYCProvider kycProvider)
        {
            _customerRepository = customerRepository;
            _customerResponseFactory = customerResponseFactory;
            _securityContext = securityContext;
            _kycProvider = kycProvider;
        }

        public async Task<Result<CustomerDto>> Handle(UpdateCustomerInfoCommand request, CancellationToken cancellationToken)
        {
            var userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if(customer == null)
            {
                return new EntityNotFoundException(CustomerErrorConsts.CustomerNotExist); 
            }
            var address = Address.Create(
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
                    request.Nationality,
                    request.Gender,
                    request.IdNumber,
                    address
                );

            customer.UpdateInfo(info);


            var kycRequest = PrepareKycClientRequest(customer);

            if(customer.KycCustomerId == null)
            {
                var response = await  _kycProvider.CreateClientAsync(kycRequest);

                customer.AddKycCustomerId(response.Id);
            }
            else
            {
                await _kycProvider.UpdateClientAsync(customer.KycCustomerId, kycRequest);
            }

            await _customerRepository.UpdateAsync(customer);

            var data = await _customerRepository.SingleAsync(x => x.Id == customer.Id);

            return await _customerResponseFactory.PrepareDto(data);
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
                    Nationality = customer.Info.Nationality,
                    SSN = customer.Info.IdNumber,
                    BirthDate = customer.Info.BirthDate,
                    Gender = customer.Info.Gender,
                    Address = customer.Info.Address
                }
            };

            return request;
        }
    }
}
