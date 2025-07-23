using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nexa.CustomerManagement.Application.Customers.Commands.AcceptCustomerInfo
{
    public class AcceptCustomerInfoCommandHandler : IApplicationRequestHandler<AcceptCustomerInfoCommand, Unit>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;

        public AcceptCustomerInfoCommandHandler(ICustomerManagementRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Unit>> Handle(AcceptCustomerInfoCommand request, CancellationToken cancellationToken)
        {
            
            var customer = await _customerRepository.AsQuerable()
                .Include(c => c.Documents)
                .ThenInclude(c => c.Attachments)
                .SingleAsync(x => x.KycCustomerId == request.KycCustomerId);


            customer.AcceptInfo();

            await _customerRepository.UpdateAsync(customer);

            return Unit.Value;
        }
    }
}
