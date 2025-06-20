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
        private readonly ICustomerManagementRepository<Document> _documentRepository;
        private readonly ISecurityContext _securityContext;
        private readonly IKYCProvider _kycProvider;
        private readonly IDocumentResponseFactory _documentResponseFactory;

        public VerifyDocumentCommandHandler(ICustomerManagementRepository<Customer> customerRepository, ICustomerManagementRepository<Document> documentRepository, ISecurityContext securityContext, IKYCProvider kycProvider, IDocumentResponseFactory documentResponseFactory)
        {
            _customerRepository = customerRepository;
            _documentRepository = documentRepository;
            _securityContext = securityContext;
            _kycProvider = kycProvider;
            _documentResponseFactory = documentResponseFactory;
        }

        public async Task<Result<DocumentDto>> Handle(VerifyDocumentCommand request, CancellationToken cancellationToken)
        {
            string userId = _securityContext.User!.Id;

            var customer = await _customerRepository.SingleOrDefaultAsync(x => x.UserId == userId);

            if(customer == null)
            {
                return new Result<DocumentDto>(new BusinessLogicException("User should complete customer application before proceeding to document verification."));
            }

            var document = await _documentRepository.SingleOrDefaultAsync(x => x.Id == request.DocumentId);

            if(document == null)
            {
                return new Result<DocumentDto>(new EntityNotFoundException(typeof(Document), request.DocumentId));

            }

            var isDocumentOwner = IsDocumentOwner(userId, document);

            if (!isDocumentOwner)
            {
                return new Result<DocumentDto>(new ForbiddenAccessException());
            }

            if (document.Status != DocumentStatus.Pending )
            {
                return new Result<DocumentDto>(new BusinessLogicException("Invalid KYC document status cannot start processing."));
            }

            var hasBothSides = document.HasAttachmentsBothSides();

            if (!hasBothSides)
            {
                 return new Result<DocumentDto>(new BusinessLogicException("KYC document should has both attachment sides (front/back) before start processing"));
            }

            var kycRequest = new KYCCheckRequest
            {
                ClientId = customer.KYCExternalId,
                DocumentId = document.KYCExternalId,
                Type = KYCCheckType.DocumentCheck
            };

            var response = await _kycProvider.CreateCheckAsync(kycRequest);

            document.Process();

            await _documentRepository.UpdateAsync(document);

            var result = await _documentRepository.SingleAsync(x => x.Id == document.Id);

            return await _documentResponseFactory.PrepareDto(result);
        }

        private bool IsDocumentOwner(string userId, Document document)
        {
            return userId == document.UserId;
        }
    }
}
