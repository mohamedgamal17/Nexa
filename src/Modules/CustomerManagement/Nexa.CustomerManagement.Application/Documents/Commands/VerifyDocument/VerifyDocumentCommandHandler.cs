using Microsoft.EntityFrameworkCore;
using Nexa.BuildingBlocks.Application.Abstractions.Security;
using Nexa.BuildingBlocks.Application.Requests;
using Nexa.BuildingBlocks.Domain.Exceptions;
using Nexa.BuildingBlocks.Domain.Results;
using Nexa.CustomerManagement.Application.Documents.Factories;
using Nexa.CustomerManagement.Domain;
using Nexa.CustomerManagement.Domain.Customers;
using Nexa.CustomerManagement.Domain.Documents;
using Nexa.CustomerManagement.Domain.KYC;
using Nexa.CustomerManagement.Shared.Dtos;
using Nexa.CustomerManagement.Shared.Enums;

namespace Nexa.CustomerManagement.Application.Documents.Commands.VerifyDocument
{
    public class VerifyDocumentCommandHandler : IApplicationRequestHandler<VerifyDocumentCommand, DocumentDto>
    {
        private readonly ICustomerManagementRepository<Customer> _customerRepository;
        private readonly IKYCProvider _kycProvider;
        private readonly ISecurityContext _securityContext;
        private readonly IDocumentResponseFactory _documentResponseFactory;

        public VerifyDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, IKYCProvider kycProvider, ISecurityContext securityContext, IDocumentResponseFactory documentResponseFactory)
        {
            _customerRepository = customerRepository;
            _kycProvider = kycProvider;
            _securityContext = securityContext;
            _documentResponseFactory = documentResponseFactory;
        }

        public async Task<Result<DocumentDto>> Handle(VerifyDocumentCommand request, CancellationToken cancellationToken)
        {
            var userId = _securityContext.User!.Id;

            var customer = await _customerRepository.AsQuerable()
                .Include(x => x.Documents)
                .ThenInclude(x => x.Attachments)
                .SingleOrDefaultAsync(x => x.UserId == userId);


            if (customer == null)
            {
                return new Result<DocumentDto>(new BusinessLogicException("Current user customer is not exist."));
            }

            var document = customer.FindDocument(request.DocumentId);

            if(document== null)
            {
                return new Result<DocumentDto>(new EntityNotFoundException(typeof(Document), request.DocumentId));
            }

            if(document.State == VerificationState.InReview 
                || document.State == VerificationState.Verified )
            {
                return new Result<DocumentDto>(new BusinessLogicException("Invalid document state."));
            }

            if (!document.CanBeVerified())
            {
                return new Result<DocumentDto>(new BusinessLogicException("Document attachments is incomplete."));
            }

            var kycRequest = new KYCCheckRequest
            {
                ClientId = customer.KycCustomerId!,
                DocumentId = document.KYCExternalId!,
                LiveVideoId = request.LiveVideoId,
                Type = KYCCheckType.IdentityCheck
            };

            await _kycProvider.CreateCheckAsync(kycRequest);

            document.Verifiy();

            await _customerRepository.UpdateAsync(customer);

            return await _documentResponseFactory.PrepareDto(document);
        }
    }
}
