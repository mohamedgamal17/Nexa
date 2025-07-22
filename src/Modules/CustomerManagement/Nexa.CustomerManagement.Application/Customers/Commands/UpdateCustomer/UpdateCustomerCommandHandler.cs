using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IApplicationRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly ISecurityContext _securityContext;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerResponseFactory _customerResponseFactory;
        private readonly IKYCProvider _kycProvider;
        public UpdateCustomerCommandHandler(ISecurityContext securityContext, ICustomerManagementRepository<Customer> customerRepository, ICustomerResponseFactory customerResponseFactory, IKYCProvider kycProvider)
        {
            _securityContext = securityContext;
            _customerRepository = customerRepository;
            _customerResponseFactory = customerResponseFactory;
            _kycProvider = kycProvider;
        }

        public async Task<Result<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
            {
                return new Result<CustomerDto>(new EntityNotFoundException("Current user dosen't have customer application"));
            }

            customer.Update(request.PhoneNumber, request.EmailAddress);

            var kycRequest = new KYCClientRequest
            {
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber
            };

            await _kycProvider.UpdateClientAsync(customer.KycCustomerId!, kycRequest);

            await _customerRepository.UpdateAsync(customer);

            var result = await _customerRepository.SingleAsync(x => x.Id == customer.Id);

            return await _customerResponseFactory.PrepareDto(result);
        }
    }
}
