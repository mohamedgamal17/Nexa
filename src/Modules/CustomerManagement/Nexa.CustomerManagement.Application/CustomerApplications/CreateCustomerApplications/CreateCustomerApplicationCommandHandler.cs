using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.CustomerApplications.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.CustomerApplications;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;
namespace Nexa.CustomerManagement.Application.CustomerApplications.CreateCustomerApplications
{
    public class CreateCustomerApplicationCommandHandler : IApplicationRequestHandler<CreateCustomerApplicationCommand, CustomerApplicationDto>
    {
        private readonly ICustomerManagementRepository<CustomerApplication> _customerApplicationRepository;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ISecurityContext _securityContext;
        private readonly IKYCProvider _kycProvider;
        private readonly ICustomerApplicationResponseFactory _customerResponseFactory;

        public CreateCustomerApplicationCommandHandler(ICustomerManagementRepository<CustomerApplication> customerApplication, ICustomerManagementRepository<Customer> customerRepository, IKYCProvider kycProvider, ISecurityContext securityContext, ICustomerApplicationResponseFactory customerResponseFactory)
        {
            _customerApplicationRepository = customerApplication;
            _customerRepository = customerRepository;
            _kycProvider = kycProvider;
            _securityContext = securityContext;
            _customerResponseFactory = customerResponseFactory;
        }

        public async Task<Result<CustomerApplicationDto>> Handle(CreateCustomerApplicationCommand request, CancellationToken cancellationToken)
        {
            var isAnyActiveApplication = await _customerApplicationRepository.AnyAsync(x => x.Status != CustomerApplicationStatus.Rejected);

            if (isAnyActiveApplication)
            {
                return new Result<CustomerApplicationDto>(new BusinessLogicException("There is already active customer application."));
            }

            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if(customer == null)
            {
                return new Result<CustomerApplicationDto>(new BusinessLogicException("Current user must create customer first."));
            }

            var kycRequest = PrepareKycRequest(request);

            var kycResponse = await _kycProvider.CreateClientAsync(kycRequest);

            var application = PrepareCustomerApplication(request, kycResponse.Id, customer.Id);

            await _customerApplicationRepository.InsertAsync(application);

            var response = await _customerResponseFactory.PrepareFullCustomerApplicationDto(application, kycResponse);

            return response;
            
        }


        private KYCClientRequest PrepareKycRequest(CreateCustomerApplicationCommand command)
        {
            var request = new KYCClientRequest
            {
                FirstName = command.FirstName,
                MiddleName = command.MiddleName,
                LastName = command.LastName,
                PhoneNumber = command.PhoneNumber,
                Nationality = command.Nationality,
                EmailAddress = command.EmailAddress,
                BirthDate = command.BirthDate,
                Gender = command.Gender,
                SSN = command.SSN,
                NationalIdentityNumber = command.NationalIdentityNumber
            };

            return request;
        }

        private CustomerApplication PrepareCustomerApplication(CreateCustomerApplicationCommand command , string kycClientId , string customerId)
        {
            var application = new CustomerApplication
            {
                KycExternalId = kycClientId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                MiddleName = command.MiddleName,
                EmailAddress = command.EmailAddress,
                PhoneNumber = command.PhoneNumber,
                Nationality = command.Nationality,
                BirthDate = command.BirthDate,
                Gender = command.Gender,
                CustomerId = customerId,
                Address = new Address
                {
                    Country = command.Address.Country,
                    City = command.Address.City,
                    State = command.Address.State,
                    StreetLine1 = command.Address.StreetLine1,
                    StreetLine2 = command.Address.StreetLine2,
                    PostalCode = command.Address.PostalCode,
                    ZipCode = command.Address.ZipCode
                },

            };

            return application;
        }
    }
}
