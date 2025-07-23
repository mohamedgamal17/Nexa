using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nexa.CustomerManagement.Application.Documents.Commands.RejectDocument
{
    public class RejectDocumentCommandHandler : IApplicationRequestHandler<RejectDocumentCommand, Unit>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;

        public RejectDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Unit>> Handle(RejectDocumentCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.AsQuerable()
                .Include(c => c.Documents)
                .ThenInclude(c => c.Attachments)
                .SingleAsync(x => x.KycCustomerId == request.KycCustomerId);

            var document = customer.Documents.Single(c => c.KYCExternalId == request.KycDocumentId);

            document.Reject();

            await _customerRepository.UpdateAsync(customer);

            return Unit.Value;
        }
    }
}
