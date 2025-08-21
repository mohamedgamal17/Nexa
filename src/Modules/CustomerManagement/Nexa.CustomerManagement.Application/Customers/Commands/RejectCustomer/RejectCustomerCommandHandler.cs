using MediatR;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Customers.Commands.RejectCustomer
{
    public class RejectCustomerCommandHandler : IApplicationRequestHandler<RejectCustomerCommand, Unit>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;

        public RejectCustomerCommandHandler(ICustomerManagementRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Unit>> Handle(RejectCustomerCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.SingleAsync(x => x.FintechCustomerId == request.FintechCustomerId);

            if(customer.State == VerificationState.Rejected)
            {
                return Unit.Value;
            }

            customer.Reject();

            await _customerRepository.UpdateAsync(customer);

            return Unit.Value;
        }
    }
}
