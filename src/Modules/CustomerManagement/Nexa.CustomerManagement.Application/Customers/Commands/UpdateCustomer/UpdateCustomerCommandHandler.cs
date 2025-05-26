using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Dtos;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nexa.CustomerManagement.Application.Customers.Commands.UpdateCustomer
{
    public class UpdateCustomerCommandHandler : IApplicationRequestHandler<UpdateCustomerCommand, CustomerDto>
    {
        private readonly ISecurityContext _securityContext;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerResponseFactory _customerResponseFactory;

        public UpdateCustomerCommandHandler(ISecurityContext securityContext, ICustomerManagementRepository<Customer> customerRepository, ICustomerResponseFactory customerResponseFactory)
        {
            _securityContext = securityContext;
            _customerRepository = customerRepository;
            _customerResponseFactory = customerResponseFactory;
        }

        public async Task<Result<CustomerDto>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if (customer == null)
            {
                return new Result<CustomerDto>(new EntityNotFoundException("Current user dosen't have customer application"));
            }


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
    }
}
