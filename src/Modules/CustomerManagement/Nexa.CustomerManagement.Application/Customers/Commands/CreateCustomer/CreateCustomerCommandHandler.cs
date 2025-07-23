using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Dtos;
namespace Nexa.CustomerManagement.Application.Customers.Commands.CreateCustomer
{
    public class CreateCustomerCommandHandler : IApplicationRequestHandler<CreateCustomerCommand, CustomerDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ISecurityContext _securityContext;
        private readonly ICustomerResponseFactory _customerResponseFactory;
        private readonly IKYCProvider _kycProvider;

        public CreateCustomerCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ISecurityContext securityContext, ICustomerResponseFactory customerResponseFactory, IKYCProvider kycProvider)
        {
            _customerRepository = customerRepository;
            _securityContext = securityContext;
            _customerResponseFactory = customerResponseFactory;
            _kycProvider = kycProvider;
        }

        public async Task<Result<CustomerDto>> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            var userId = _securityContext.User!.Id;

            var userAlreadHasCustomer = await _customerRepository.AnyAsync(x => x.UserId == userId);

            if (userAlreadHasCustomer)
            {
                return new Result<CustomerDto>(new BusinessLogicException("User customer is not already created."));
            }

            var customer = new Customer(userId,request.PhoneNumber,request.EmailAddress);

            var kycRequest = new KYCClientRequest
            {
                EmailAddress = customer.EmailAddress,
                PhoneNumber = customer.PhoneNumber
            };

            var kycClient = await _kycProvider.CreateClientAsync(kycRequest);

            customer.AddKycCustomerId(kycClient.Id);

            await _customerRepository.InsertAsync(customer);

            var result = await _customerRepository.SingleAsync(x => x.Id == customer.Id);

            return await _customerResponseFactory.PrepareDto(result);
        }
    }
}
