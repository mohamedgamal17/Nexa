using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Customers.Commands.VerifyCustomerInfo
{
    public class VerifyCustomerInfoCommandHandler : IApplicationRequestHandler<VerifyCustomerInfoCommand, CustomerDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ISecurityContext _securityContext;
        private readonly IKYCProvider _kycProvider;
        private readonly ICustomerResponseFactory _customerReponseFactory;
        public VerifyCustomerInfoCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ISecurityContext securityContext, IKYCProvider kycProvider, ICustomerResponseFactory customerReponseFactory)
        {
            _customerRepository = customerRepository;
            _securityContext = securityContext;
            _kycProvider = kycProvider;
            _customerReponseFactory = customerReponseFactory;
        }

        public async Task<Result<CustomerDto>> Handle(VerifyCustomerInfoCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;
            
            var customer = await _customerRepository
                .AsQuerable()
                .Include(x=> x.Documents)
                .ThenInclude(x=> x.Attachments)
                .SingleOrDefaultAsync(x => x.UserId == userId);

            if(customer == null)
            {
                return new Result<CustomerDto>(new BusinessLogicException("User customer is not already created."));
            }

            if(customer.Info == null)
            {
                return new Result<CustomerDto>(new BusinessLogicException("Customer should complete peronal information before verifing data"));
            }

            if(customer.InfoVerificationState == VerificationState.InReview)
            {
                return new Result<CustomerDto>(new BusinessLogicException("Verification process for current customer is already running"));
            }   

            if(customer.InfoVerificationState == VerificationState.Verified)
            {
                return new Result<CustomerDto>(new BusinessLogicException("Customer info is already accepted cannot recheck"));
            }

            var kycRequest = new KYCCheckRequest
            {
                ClientId = customer.KycCustomerId!,
                Type = KYCCheckType.IdNumberCheck
            };

            await _kycProvider.CreateCheckAsync(kycRequest);

            customer.VerifyInfo();

            await _customerRepository.UpdateAsync(customer);

            return await _customerReponseFactory.PrepareDto(customer);
        }
    }
}
