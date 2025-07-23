using MediatR;
using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;

namespace Nexa.CustomerManagement.Application.Documents.Commands.AcceptDocument
{
    public class AcceptDocumentCommandHandler : IApplicationRequestHandler<AcceptDocumentCommand, Unit>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;

        public AcceptDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public async Task<Result<Unit>> Handle(AcceptDocumentCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.AsQuerable()
                .Include(c => c.Documents)
                .ThenInclude(x => x.Attachments)
                .SingleAsync(x => x.KycCustomerId == request.KycCustomeId);

            var document = customer.Documents.Single(x => x.KYCExternalId == request.KycDocumentId);

            document.Accept();

            await _customerRepository.UpdateAsync(customer);

            return Unit.Value;
        }
    }
}
