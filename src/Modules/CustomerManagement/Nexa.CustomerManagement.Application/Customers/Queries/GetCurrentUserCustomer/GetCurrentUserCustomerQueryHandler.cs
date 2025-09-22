using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Customers.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Dtos;

namespace Nexa.CustomerManagement.Application.Customers.Queries.GetCurrentUserCustomer
{
    public class GetCurrentUserCustomerQueryHandler : IApplicationRequestHandler<GetCurrentUserCustomerQuery, CustomerDto>
    {
        private readonly ISecurityContext _securityContext;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly ICustomerResponseFactory _customerResponseFactory;

        public GetCurrentUserCustomerQueryHandler(ISecurityContext securityContext, ICustomerManagementRepository<Customer> customerRepository, ICustomerResponseFactory customerResponseFactory)
        {
            _securityContext = securityContext;
            _customerRepository = customerRepository;
            _customerResponseFactory = customerResponseFactory;
        }

        public async Task<Result<CustomerDto>> Handle(GetCurrentUserCustomerQuery request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if(customer == null)
            {
                return new Result<CustomerDto>(new BusinessLogicException("current user must complete customer data first."));
            }

            return await _customerResponseFactory.PrepareDto(customer);
        }
    }
}
