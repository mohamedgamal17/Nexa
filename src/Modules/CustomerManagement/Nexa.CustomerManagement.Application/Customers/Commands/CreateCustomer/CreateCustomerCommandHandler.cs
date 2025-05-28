using MediatR;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Dtos;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
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
                return new Result<CustomerDto>(new BusinessLogicException("User customer application already created."));
            }

            var kycRequest = PrepareKYCClientRequest(request);

            var kycClient = await _kycProvider.CreateClientAsync(kycRequest); 

            var customer = new Customer();

            PrepareCustomerEntity(customer, userId, kycClient.Id ,request);

            await _customerRepository.InsertAsync(customer);

            var result = await _customerRepository.SingleAsync(x => x.Id == customer.Id);

            return await _customerResponseFactory.PrepareDto(result);
        }


        private void PrepareCustomerEntity(Customer customer, string userId,string externalId  ,CreateCustomerCommand command)
        {
            customer.ExternalId = externalId;
            customer.UserId = userId;
            customer.FirstName = command.FirstName;
            customer.LastName = command.LastName;
            customer.MiddleName = command.MiddleName;
            customer.Nationality = command.Nationality;
            customer.BirthDate = command.BirthDate;
            customer.PhoneNumber = command.PhoneNumber;
            customer.EmailAddress = command.EmailAddress;
            customer.Gender = command.Gender;
            customer.SocialSecurityNumber = command.SocialSecurityNumber;
            customer.SocialInsuranceNumber = command.SocialInsuranceNumber;
            customer.TaxIdentificationNumber = command.TaxIdentificationNumber;
            customer.NationalIdentityNumber = command.NationalIdentityNumber;

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

        private KYCClientRequest PrepareKYCClientRequest(CreateCustomerCommand command)
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
