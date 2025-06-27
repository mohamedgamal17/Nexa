using Microsoft.AspNetCore.Authorization;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.CustomerManagement.Domain.CustomerApplications;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain;

namespace Nexa.CustomerManagement.Application.CustomerApplications.Policies
{
    public class IsCustomerApplicationOwnerRequirment : IAuthorizationRequirement { }

    public class IsCustomerApplicationOwnerRequirmentHandler : AuthorizationHandler<IsCustomerApplicationOwnerRequirment, CustomerApplication>
    {

        private readonly ISecurityContext _securityContext;
        private readonly ICustomerManagementRepository<Customer> _customerRepository;

        public IsCustomerApplicationOwnerRequirmentHandler(ISecurityContext securityContext, ICustomerManagementRepository<Customer> customerRepository)
        {
            _securityContext = securityContext;
            _customerRepository = customerRepository;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IsCustomerApplicationOwnerRequirment requirement, CustomerApplication resource)
        {
            string userId = _securityContext.User!.Id;
            var customer = await _customerRepository.SingleAsync(x => x.UserId == userId);

            if (customer.Id == resource.CustomerId)
            {
                context.Succeed(requirement);
            }
        }
    }
}
