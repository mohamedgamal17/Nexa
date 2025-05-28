using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Dtos;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
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

            var kycRequest = PrepareKYCClientRequest(request);

            var kycResponse = await _kycProvider.UpdateClientAsync(customer.KYCExternalId, kycRequest);

            PrepareCustomerEntity(customer, request);

            await _customerRepository.UpdateAsync(customer);

            var result = await _customerRepository.SingleAsync(x => x.Id == customer.Id);

            return await _customerResponseFactory.PrepareDto(result);
        }

        private void PrepareCustomerEntity(Customer customer, UpdateCustomerCommand command)
        {
            customer.FirstName = command.FirstName;
            customer.LastName = command.LastName;
            customer.MiddleName = command.MiddleName;
            customer.Nationality = command.Nationality;
            customer.BirthDate = command.BirthDate;
            customer.PhoneNumber = command.PhoneNumber;
            customer.EmailAddress = command.EmailAddress;
            customer.Gender = command.Gender;


            if (command.Address != null)
            {
                customer.Address = new Address
                {
                    Country = command.Address.Country,
                    City = command.Address.City,
                    State = command.Address.State,
                    StreetLine1 = command.Address.StreetLine1,
                    StreetLine2 = command.Address.StreetLine2,
                    PostalCode = command.Address.PostalCode,
                    ZipCode = command.Address.ZipCode
                };
            }
        }

        private KYCClientRequest PrepareKYCClientRequest(UpdateCustomerCommand command)
        {
            var request = new KYCClientRequest
            {
                FirstName = command.FirstName,
                MiddleName = command.MiddleName,
                LastName = command.LastName,
                PhoneNumber = command.PhoneNumber,
                EmailAddress = command.EmailAddress,
                BirthDate = command.BirthDate,
                Gender = command.Gender,
                Nationality = command.Nationality
            };

            return request;
        }
    }
}
