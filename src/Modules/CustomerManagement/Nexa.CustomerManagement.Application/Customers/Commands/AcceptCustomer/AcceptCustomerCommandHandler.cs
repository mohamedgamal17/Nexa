using MediatR;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Customers.Commands.AcceptCustomer
{
    public class AcceptCustomerCommandHandler : IApplicationRequestHandler<AcceptCustomerCommand, Unit>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;

        public AcceptCustomerCommandHandler(ICustomerManagementRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Unit>> Handle(AcceptCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository
                .SingleAsync(x => x.FintechCustomerId == request.FintechCustomerId);

            if(customer.State == VerificationState.Verified)
            {
                return Unit.Value;
            }

            customer.Accept();

            await _customerRepository.UpdateAsync(customer);

            return Unit.Value;
        }
    }
}
